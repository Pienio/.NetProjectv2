using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DatabaseAccess.Model;
using Visits.Utils;

namespace AdminPanel.ViewModels
{
    class MainViewModel : ViewModel
    {
        private List<ProfileRequest> _requests;
        private List<Specialization> _specializations;

        public List<ProfileRequest> Requests
        {
            get { return _requests; }
            set { _requests = value; OnPropertyChanged(nameof(Requests)); }
        }
        public List<Specialization> Specializations
        {
            get { return _specializations; }
            set { _specializations = value; OnPropertyChanged(nameof(Specializations)); }
        }

        public async Task Initialize()
        {
            _service.Fill();

            var requests = await _service.GetRequestsAsync();
            Requests = requests.ToList();

            var specs = await _service.GetSpecializationsListAsync();
            Specializations = specs.ToList();
        }

        public ICommand AcceptCommand => new Command(async p =>
        {
            ProfileRequest request = p as ProfileRequest;
            bool res;
            if (request.OldProfile == null)
            {
                request.NewProfile.ProfileAccepted = true;
                res = await _service.UpdateDoctorAsync(request.NewProfile);
            }
            else
            {
                request.OldProfile.CopyFrom(request.NewProfile);
                res = await _service.UpdateDoctorAsync(request.OldProfile);
            }
            if (res)
            {
                res = await _service.DeleteRequestAsync(request);
                if (res)
                    Requests.Remove(request);
            }
            if (!res)
            {
                MessageBox.Show("Nie udało się dokonać zmian z powodu błędu. Spróbuj ponownie.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var cd = await _service.GetRequestsAsync();
            Requests = cd.ToList();
            //wyślij maila
        });
        public ICommand RejectCommand => new Command(async p =>
        {
            ProfileRequest request = p as ProfileRequest;
            bool res;

            TextWindow wnd = new TextWindow();
            if (!wnd.ShowDialog().GetValueOrDefault(false))
                return;
            res = await _service.DeleteDoctorAsync(request.NewProfile);
            if (res)
            {
                res = await _service.DeleteRequestAsync(request);
                if (res)
                    Requests.Remove(request);
            }
            if (!res)
            {
                MessageBox.Show("Nie udało się dokonać zmian z powodu błędu. Spróbuj ponownie.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string reason = wnd.TextInserted;
            OnPropertyChanged(nameof(Requests));
            // wyślij maila
        });
        public ICommand RefreshCommand => new Command(async p =>
        {
            await Initialize();
        });
        public ICommand AddSpecializationCommand => new Command(async p =>
        {
            TextWindow wnd = new TextWindow();
            wnd.Title = "Nazwa nowej specjalizacji";
            if (!wnd.ShowDialog().GetValueOrDefault(false))
                return;
            if (Specializations.Any(s => s.Name == wnd.TextInserted))
            {
                MessageBox.Show("Specjalizacja o podanej nazwie już istnieje", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool res = await _service.AddSpecializationAsync(new Specialization(wnd.TextInserted));
            if (!res)
            {
                MessageBox.Show("Nie udało się dodać specjalizacji z powodu błędu. Spróbuj ponownie.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var specs = await _service.GetSpecializationsListAsync();
            Specializations = specs.ToList();
        });
        public ICommand UpdateSpecializationCommand => new Command(async p =>
        {
            Specialization spec = p as Specialization;
            TextWindow wnd = new TextWindow() { TextInserted = spec.Name };
            wnd.Title = "Nowa nazwa specjalizacji";
            wnd.TextInserted = spec.Name;
            if (!wnd.ShowDialog().GetValueOrDefault(false))
                return;
            if (Specializations.Any(s => s.Name == wnd.TextInserted && s.Key != spec.Key))
            {
                MessageBox.Show("Specjalizacja o podanej nazwie już istnieje", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            spec.Name = wnd.TextInserted;
            bool res = await _service.UpdateSpecializationAsync(spec);
            if (!res)
            {
                MessageBox.Show("Nie udało się zaktualizować specjalizacji z powodu błędu. Spróbuj ponownie.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var specs = await _service.GetSpecializationsListAsync();
            Specializations = specs.ToList();
        });
        public ICommand DeleteSpecializationCommand => new Command(async p =>
        {
            bool res = await _service.DeleteSpecializationAsync(p as Specialization);
            if (!res)
            {
                MessageBox.Show("Nie udało się usunąć specjalizacji, ponieważ istnieją lekarze z daną specjalizacją.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var specs = await _service.GetSpecializationsListAsync();
            Specializations = specs.ToList();
        });
    }
}
