using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Visits.DataServiceReference;
using Visits.Services;
using Visits.Utils;
using MailService;

namespace Visits.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private IEnumerable<Doctor> _doctors;
        private IEnumerable<Specialization> _specializations;
        private Doctor _selectedDoctor;
        private Specialization _selectedSpecialization;
        private string _searchByName;

        public string LoggedUserName => _loggedUser?.Logged?.User?.Name?.ToString();

        public IEnumerable<Doctor> Doctors
        {
            get { return _doctors; }
            set { _doctors = value; OnPropertyChanged(nameof(Doctors)); OnPropertyChanged(nameof(AnyDoctors)); }
        }
        public IEnumerable<Specialization> Specializations
        {
            get { return _specializations; }
            set { _specializations = value; OnPropertyChanged(nameof(Specializations)); }
        }
        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set { _selectedDoctor = value; OnPropertyChanged(nameof(SelectedDoctor)); }
        }
        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set { _selectedSpecialization = value; OnPropertyChanged(nameof(SelectedSpecialization)); }
        }
        public string SearchByNameText
        {
            get { return _searchByName; }
            set { _searchByName = value; OnPropertyChanged(nameof(SearchByNameText)); }
        }

        public bool AnyDoctors => _doctors == null ? false : _doctors.Count() > 0;

        public MainWindowViewModel(ILogUserService user) : base(user)
        {
            _loggedUser.LoggedChanged += (o, e) => OnPropertyChanged(nameof(LoggedUserName));
        }

        public ICommand LoginCmd => new Command(p =>
        {

            if (_loggedUser.Logged == null)
            {
                
                var wnd = App.Container.Resolve<Login>();
                wnd.ShowDialog();

            }
            else
            {
                _loggedUser.LogOut();
            }
            OnPropertyChanged(nameof(LoggedUserName));
        });

        public ICommand RegisterCmd => new Command(parameter =>
        {

            var lekpac = new LekPac();
            lekpac.ShowDialog();
            if (lekpac.GetResult() != 0)
            {
               
                var wnd = App.Container.Resolve<Register>();
                if (lekpac.GetResult() == 2)
                    wnd.WH = false;
                else
                    wnd.WH = true;
                wnd.Show();
            }
        });

        public ICommand RegisterVisitCmd => new Command(parameter =>
        {
       
            var wnd = App.Container.Resolve<RegVisit>();
           wnd.SelectedDoctor = SelectedDoctor;
            wnd.Show();
        }, p =>
        {
            return !(_loggedUser.Logged is Doctor) && SelectedDoctor != null;
        });

        //async 
        public ICommand SearchCmd => new Command(p =>
        {
            Doctors = _service.SearchDoctorsList(SelectedSpecialization, SearchByNameText);
            foreach (var VARIABLE in Doctors)
            {
                VARIABLE.FirstFreeSlot = _service.GetFirstFreeSlot((int)VARIABLE.Key);
            }
           
        });

        public ICommand EditProfileCmd => new Command(parameter =>
        {
           
            var wnd = App.Container.Resolve<Edit>();
            wnd.ShowDialog();
            OnPropertyChanged(nameof(LoggedUserName));
            SearchCmd.Execute(null);
            Specializations = _service.GetSpecializationsList();
        });

        public ICommand VisitsViewCmd => new Command(p =>
        {
            
            var wnd = App.Container.Resolve<WizList>();
            wnd.ShowDialog();
        });

        public ICommand ClearFilters => new Command(p =>
        {
            SelectedSpecialization = null;
            SearchByNameText = "";

            SearchCmd.Execute(null);
            
        });

        public void Initialize()
        {
            _service.Fill();
            Specializations = _service.GetSpecializationsList();
            Doctors = _service.GetDoctorsList();
            foreach (var VARIABLE in Doctors)
            {
                VARIABLE.FirstFreeSlot = _service.GetFirstFreeSlot((int)VARIABLE.Key);
            }
           
        }
    }
}
