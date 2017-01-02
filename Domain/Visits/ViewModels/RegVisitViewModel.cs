using DatabaseAccess;
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
using MailService;
using Visits.Services;
using Microsoft.Practices.Unity;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class RegVisitViewModel : ViewModel
    {
        private Doctor _currentDoctor;
        private Week _currentWeek;

        private Patient LoggedPatient => _loggedUser.Logged as Patient;

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
        public bool AnyVisits => _currentWeek != null && _currentWeek.Days.Length > 0;

        public ICommand ChangeWeekCmd => new Command(async p => CurrentWeek = await Week.Create(CurrentDoctor, CurrentWeek.From.AddDays(int.Parse(p.ToString()))));
        public ICommand RegisterVisitCmd => new Command(async p =>
        {
            var selectedDate = (DateTime)p;

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

        bool contains = await Task.Run(() => ((_service.GetDoctorVisits((int)CurrentDoctor.Key,false)).Where(d=>d.Date==selectedDate)).Any());
            if (contains)
                MessageBox.Show("Dany termin został już zajęty. Nastąpi odświeżenie widoku", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                await Task.Run(() => _service.RegisterVisit(selectedDate,(int)LoggedPatient.Key,(int)CurrentDoctor.Key));

            CurrentDoctor =await _service.GetDoctorByIdAsync((int)CurrentDoctor.Key);
            CurrentWeek =await Week.Create(CurrentDoctor,CurrentWeek.Days[0].Date);
            if (!contains)
            {
                MessageBox.Show("Wizyta została zarejestrowana", App.ResourceAssembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
                MailServices ans = new MailServices();
                ans.SendVisitRegistrationNotifications(CurrentDoctor, LoggedPatient, selectedDate);
            }
        });

        public RegVisitViewModel(ILogUserService user) : base(user)
        {
            if (user.Logged != null && !(user.Logged is Patient))
                throw new InvalidOperationException("Widok rejestracji wizyt jest dostępny tylko dla pacjentów i anonimowych użytkowników.");
        }

        public  async Task Load()
        {
            var first = _service.GetFirstFreeSlot((int)CurrentDoctor.Key);
            first = first.AddDays(-Week.DayOfWeekNo(first));
            var a = _service.GetDoctorById((int)CurrentDoctor.Key);
            CurrentWeek =await Week.Create(a, first);
        }

        public void Initialize(Doctor doctor) //async 
        {
            CurrentDoctor = doctor;
            Load();
        }
    }
}
