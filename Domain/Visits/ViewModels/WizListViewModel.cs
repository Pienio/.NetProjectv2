﻿using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MailService;
using Visits.Services;
using Visits.Utils;

namespace Visits.ViewModels
{
    public class WizListViewModel : ViewModel
    {
        private IEnumerable<Visit> _visits;
        private VisitsType _selectedType;

        public WizListViewModel(ILogUserService user) : base(user) { }

        public Person LoggedUser => _loggedUser.Logged;
        public bool AnyVisits => _visits == null ? false : _visits.Count() > 0;

        public IEnumerable<Visit> Visits
        {
            get { return _visits; }
            set
            {
                _visits = value;
                OnPropertyChanged(nameof(Visits));
                OnPropertyChanged(nameof(AnyVisits));
            }
        }

        public bool CanUserDeleteVisits => SelectedType == VisitsType.Planowane;
        public VisitsType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
                OnPropertyChanged(nameof(CanUserDeleteVisits));
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                SetVisits();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public ICommand DeleteVisitCmd => new Command(async p =>
        {
            Visit v = p as Visit;
            if (MessageBox.Show("Czy na pewno chcesz odwołać zaznaczoną wizytę?", App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            bool a= await _service.DeleteVisitAsync(v);
            await SetVisits();
            if (a)
            {
                MessageBox.Show("Odwołano wizytę z powodzeniem", App.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                MailServices ans = new MailServices();
                ans.SendVisitDeleteNotification(v, LoggedUser.User.Kind);
            }
           
            else
            {
                MessageBox.Show("Wystąpił błąd podczas odwoływania wizyty", App.Name, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });

        public void Invoke()
        {
            SetVisits().Wait();
        }
        private async Task SetVisits()
        {
            DateTime now = DateTime.Now;
            IEnumerable<Visit> visits = null;
            if (_loggedUser.Logged is Patient)
            {
                visits = await Task.Run(() => _service.GetPatientVisits((int)_loggedUser.Logged.Key, (SelectedType == VisitsType.Archiwalne ? true : false)));
                foreach (var VARIABLE in visits)
                {
                    VARIABLE.Doctor = _service.GetDoctorById((int)VARIABLE.Doctor.Key);
                }
            }
            else if (_loggedUser.Logged is Doctor)
            {
                visits = await Task.Run(() => _service.GetDoctorVisits((int)_loggedUser.Logged.Key, (SelectedType == VisitsType.Archiwalne ? true : false)));
                foreach (var VARIABLE in visits)
                {
                    VARIABLE.Patient = _service.GetPatientById((int)VARIABLE.Patient.Key);
                }
            }

            Visits = visits;
        }

        public enum VisitsType { Planowane, Archiwalne }
    }
}
