﻿using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Visits.Services;
using Microsoft.Practices.Unity;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class RegVisitViewModel : ViewModel
    {
        private Doctor _currentDoctor;
        private Week _currentWeek;

        private Patient LoggedPatient
        {
            get { return _loggedUser.Logged as Patient; }
        }

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set { _currentDoctor = value; OnPropertyChanged(nameof(CurrentDoctor)); }
        }
        public Week CurrentWeek
        {
            get { return _currentWeek; }
            set { _currentWeek = value; OnPropertyChanged(nameof(CurrentWeek)); OnPropertyChanged(nameof(AnyVisits)); }
        }
        public bool AnyVisits => _currentWeek == null ? false : _currentWeek.Days.Length > 0;

        public ICommand ChangeWeekCmd => new Command(async p => CurrentWeek = await Week.Create(CurrentDoctor, CurrentWeek.From.AddDays(int.Parse(p.ToString())), _applicationDataFactory.CreateApplicationData()));
        public ICommand RegisterVisitCmd => new Command(async p =>
        {
            var selectedDate = (DateTime)p;
            var db = _applicationDataFactory.CreateApplicationData();
            if (LoggedPatient == null)
            {

                var wnd = App.Container.Resolve<Login>();
                wnd.ShowDialog();
                if (_loggedUser.Logged == null)
                    return;

                if (!(_loggedUser.Logged is Patient))
                {
                    MessageBox.Show("Tylko pacjenci mogą rejestrować się na wizyty", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    _loggedUser.LogOut();
                    return;
                }
            }
            if (MessageBox.Show(string.Format("Czy na pewno chcesz zarejestrować się do {0} na termin dnia {1:dd.MM.yyyy} o godzinie {1:HH:mm}?",
                CurrentDoctor.User.Name, selectedDate), App.ResourceAssembly.GetName().Name,
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            //dodac sprawdzenie, czy na pewno dany termin jest wolny

            var db1 = _applicationDataFactory.CreateTransactionalApplicationData();
            bool contains = await Task.Run(() => (from v in db.Visits
                                                  where v.Date == selectedDate && v.Doctor.Key == CurrentDoctor.Key
                                                  select v).Any());
            if (contains)
                MessageBox.Show("Dany termin został już zajęty. Nastąpi odświeżenie widoku", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                await Task.Run(() => LoggedPatient.Visits.Add(new Visit(LoggedPatient, (from d in db1.Doctors where d.Key == CurrentDoctor.Key select d).First(), selectedDate)));
            db1.Commit();
            CurrentWeek = await Week.Create(CurrentDoctor, CurrentWeek.Days[0].Date, db1);
            if (!contains)
                MessageBox.Show("Wizyta została zarejestrowana", App.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        });

        public RegVisitViewModel(ILogUserService user, IApplicationDataFactory factory) : base(factory, user)
        {
            if (user.Logged != null && !(user.Logged is Patient))
                throw new InvalidOperationException("Widok rejestracji wizyt jest dostępny tylko dla pacjentów i anonimowych użytkowników.");
        }

        public async Task Load()
        {
            var first = CurrentDoctor.FirstFreeSlot;
            first = first.AddDays(-Week.DayOfWeekNo(first));
            CurrentWeek = await Week.Create(CurrentDoctor, first.Date, _applicationDataFactory.CreateApplicationData());
        }

        private async Task AddVisit(Visit item, ITransactionalApplicationData context)
        {
            await Task.Run(() => context.Visits.Add(item));
        }

        public async void Initialize(Doctor doctor)
        {
            CurrentDoctor = doctor;
            await Load();
        }

        public class Week
        {
            public Day[] Days { get; }
            public DateTime From { get; }
            public string Title => string.Format("Aktualny tydzień: {0:dd.MM.yyyy} - {1:dd.MM.yyyy}", From, From.AddDays(6));

            public Week(DateTime monday, Day[] days)
            {
                Days = days;
                From = monday;
            }

            public async static Task<Week> Create(Doctor doc, DateTime monday, IApplicationData db)
            {
                if (doc == null)
                    throw new ArgumentNullException(nameof(doc));
                int i = 0;
                List<Day> days = new List<Day>();
                foreach (var time in doc.WeeklyWorkingTime)
                {
                    List<DateTime> slots = new List<DateTime>();
                    DateTime current = monday.AddDays(i - DayOfWeekNo(monday));
                    if (time != null)
                    {
                        current = new DateTime(current.Year, current.Month, current.Day, time.Start, 0, 0);
                        var visits = await Task.Run(() => (from v in doc.Visits
                                                           where v.Date.Year == current.Year && v.Date.Month == current.Month && v.Date.Day == current.Day
                                                           select v.Date));
                        for (DateTime s = current; s.Hour < time.End; s = s.AddMinutes(30))
                            if (!visits.Contains(s) && s >= DateTime.Now.AddHours(1))
                                slots.Add(s);
                    }
                    if (slots.Count > 0)
                        days.Add(new Day(current.Date, slots.ToArray()));
                    i++;
                }
                return new Week(monday, days.ToArray());
            }

            public static int DayOfWeekNo(DateTime day)
            {
                switch (day.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return 0;
                    case DayOfWeek.Tuesday:
                        return 1;
                    case DayOfWeek.Wednesday:
                        return 2;
                    case DayOfWeek.Thursday:
                        return 3;
                    case DayOfWeek.Friday:
                        return 4;
                }
                throw new ArgumentException("Dzień nie może być sobotą ani niedzielą.", nameof(day));
            }
        }

        public class Day
        {
            public string DayName => new CultureInfo("pl-PL").DateTimeFormat.GetDayName(Date.DayOfWeek);
            public DateTime Date { get; }
            public DateTime[] Slots { get; }

            public Day(DateTime date, DateTime[] slots)
            {
                Date = date;
                Slots = slots;
            }
        }
    }
}
