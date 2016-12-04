using DatabaseAccess;
using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Visits.Services;
using Microsoft.Practices.Unity;
using Visits.Utils;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Visits.ViewModels
{
    public class EditViewModel : ViewModel, IDataErrorInfo
    {
        private User _User;
        private IEnumerable<Specialization> _SpecList;
        private Patient _Patient;
        private Doctor _newDoctor;
        private Doctor _oldDoctor;
        private string _pas = "";
        private bool _Who;

        public EditViewModel(ILogUserService user) : base(user) { }

        public bool Who
        {
            get { return _Who; }
            set
            {
                _Who = value;
                OnPropertyChanged("Who");

            }
        }
        public string Error
        {
            get { return String.Empty; }
        }

        public string this[string fieldName]
        {
            get
            {
                string result = null;

                if (fieldName == "Mail")
                {
                    if (string.IsNullOrWhiteSpace(Mail))
                        result = "E-mail nie może być pusty";
                    string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(Mail))
                        result = "Niepoprawna forma adresu e=mail.";
                }
                if (fieldName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        result = "Imię nie może być puste!";
                }
                if (fieldName == "LastName")
                {
                    if (string.IsNullOrEmpty(LastName))
                        result = "Nazwisko nie może być puste!";
                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 1)
                        result = "Prosze wprowadzić hasło, aby zatwierdzić zmiany!";

                }
                if (fieldName == "PE")
                {

                    if (PE <= PS)
                        result = "Godzina końcowa musi być większa od początkowej";
                }
                if (fieldName == "WE")
                {

                    if (WE <= WS)
                        result = "Godzina końcowa musi być większa od początkowej";
                }
                if (fieldName == "SE")
                {

                    if (SE <= SS)
                        result = "Godzina końcowa musi być większa od początkowej";
                }
                if (fieldName == "CE")
                {

                    if (CE <= CS)
                        result = "Godzina końcowa musi być większa od początkowej";
                }
                if (fieldName == "PIE")
                {

                    if (PIE <= PIS)
                        result = "Godzina końcowa musi być większa od początkowej";
                }

                return result;
            }
        }
        public string FirstName
        {
            get { return _User.Name.Name; }
            set
            {
                _User.Name.Name = value;
                OnPropertyChanged("FirstName");

            }
        }

        public string LastName
        {
            get { return _User.Name.Surname; }
            set
            {
                _User.Name.Surname = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Pesel
        {
            get { return _User.PESEL; }
        }

        public string Mail
        {
            get { return _User.Mail; }
            set
            {
                _User.Mail = value;
                OnPropertyChanged(nameof(Mail));
            }
        }
        public string Pas
        {
            get { return _pas; }
            set
            {
                _pas = value;
                OnPropertyChanged("Pas");
            }
        }

        public IEnumerable<Specialization> SpecList
        {
            get { return _SpecList; }
            set
            {
                _SpecList = value;
                OnPropertyChanged("SpecList");
            }
        }
        public Specialization Spec
        {
            get { return _newDoctor.Specialization; }
            set
            {
                _newDoctor.Specialization = value;
                OnPropertyChanged("Spec");
            }
        }

        public int PS
        {
            get { return _newDoctor.MondayWorkingTime.Start; }
            set
            {

                _newDoctor.MondayWorkingTime.Start = value;
                OnPropertyChanged("PS");
            }
        }
        public int PE
        {
            get { return _newDoctor.MondayWorkingTime.End; }
            set
            {
                _newDoctor.MondayWorkingTime.End = value;
                OnPropertyChanged("PE");
            }
        }
        public int WS
        {
            get { return _newDoctor.TuesdayWorkingTime.Start; }
            set
            {
                _newDoctor.TuesdayWorkingTime.Start = value;
                OnPropertyChanged("WS");
            }
        }
        public int WE
        {
            get { return _newDoctor.TuesdayWorkingTime.End; }
            set
            {
                _newDoctor.TuesdayWorkingTime.End = value;
                OnPropertyChanged("WE");
            }
        }
        public int SS
        {
            get { return _newDoctor.WednesdayWorkingTime.Start; }
            set
            {
                _newDoctor.WednesdayWorkingTime.Start = value;
                OnPropertyChanged("SS");
            }
        }
        public int SE
        {
            get { return _newDoctor.WednesdayWorkingTime.End; }
            set
            {
                _newDoctor.WednesdayWorkingTime.End = value;
                OnPropertyChanged("SE");
            }
        }
        public int CS
        {
            get { return _newDoctor.ThursdayWorkingTime.Start; }
            set
            {
                _newDoctor.ThursdayWorkingTime.Start = value;
                OnPropertyChanged("CS");
            }
        }
        public int CE
        {
            get { return _newDoctor.ThursdayWorkingTime.End; }
            set
            {
                _newDoctor.ThursdayWorkingTime.End = value;
                OnPropertyChanged("CE");
            }
        }
        public int PIS
        {
            get { return _newDoctor.FridayWorkingTime.Start; }
            set
            {
                _newDoctor.FridayWorkingTime.Start = value;
                OnPropertyChanged("PIS");
            }
        }
        public int PIE
        {
            get { return _newDoctor.FridayWorkingTime.End; }
            set
            {
                _newDoctor.FridayWorkingTime.End = value;
                OnPropertyChanged("PIE");
            }
        }


        public ICommand ChangePass => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pas = PasswordHasher.CreateHash(a.Password);

        });
        public ICommand Close => new Command(p =>
        {

            Window k = p as Window;
            k.Close();

        });
        public ICommand ChangePassword => new Command(p =>
        {

            //var db = _applicationDataFactory.CreateApplicationData();
            var wnd = App.Container.Resolve<ChangePass>();
            wnd.ShowDialog();


        });
        public ICommand UpdateUser => new Command(async p =>
        {
            if (Pas != _User.Password)
            {
                MessageBox.Show("Hasło nieprawidłowe");
                Pas = "";
                return;
            }

            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                bool numSign = r.Next(2) == 1;
                sb.Append(numSign ? (char)r.Next(49, 58) : (char)r.Next(65, 91));
            }
            MailService.MailServices mail = new MailService.MailServices();
            string token = sb.ToString();
            mail.SendRegistrationConfirmation(Mail, token);
            //okienko do wpisania tokena

            if (_User.Kind == DocOrPat.Doctor)
            {
                bool res = await _service.AddRequestAsync(new ProfileRequest(_oldDoctor, _newDoctor));
                if (!res)
                {
                    MessageBox.Show("Poprzednia prośba o edycję profilu nie została jeszcze rozpatrzona.");
                    return;
                }
            }
            else
            {
                _service.UpdatePatient(_Patient);
            }
            Window k = p as Window;
            k.Close();
        });
        public ICommand DeleteUser => new Command(p =>
        {
            if (Pas == _User.Password)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć konto", App.Name, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

                //var db = _applicationDataFactory.CreateTransactionalApplicationData();
                if (_loggedUser.Logged is Patient)
                {
                    _service.DeletePatient(_loggedUser.Logged as Patient);
                    //Patient asd = _loggedUser.Logged as Patient;
                    //IEnumerable<Visit> obw = _service.GetPatientVisits((int)asd.Key,true);
                    //if (obw.ToList().Count == 0)
                    //{
                    //    User adfg = asd.User;
                    //    db.Patients.Attach(asd);
                    //    db.Patients.Remove(asd);
                    //    db.Users.Attach(adfg);
                    //    db.Users.Remove(adfg);
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < asd.Visits.Count; i++)
                    //    {
                    //        if (asd.Visits[i].Date > DateTime.Now)
                    //        {
                    //            db.Visits.Attach(asd.Visits[i]);
                    //            db.Visits.Remove(asd.Visits[i]);

                    //        }
                    //    }
                    //    asd.User.Active = false;
                    //}
                }
                else
                {
                    _service.DeleteDoctor(_loggedUser.Logged as Doctor);
                    //Doctor asd = _loggedUser.Logged as Doctor;
                    //IEnumerable<Visit> obw = db.Visits.Select(d => d).Where(d => d.Doctor.Key == asd.Key);
                    //if (obw.ToList().Count == 0)
                    //{
                    //    User adfg = asd.User;
                    //    db.Doctors.Attach(asd);
                    //    db.Doctors.Remove(asd);
                    //    db.Users.Attach(adfg);
                    //    db.Users.Remove(adfg);
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < asd.Visits.Count; i++)
                    //    {
                    //        if (asd.Visits[i].Date > DateTime.Now)
                    //        {
                    //            db.Visits.Attach(asd.Visits[i]);
                    //            db.Visits.Remove(asd.Visits[i]);

                    //        }
                    //    }
                    //    asd.User.Active = false;
                    //}
                }
                _loggedUser.LogOut();
                // db.Commit();




                Window k = p as Window;
                k.Close();
            }
            else
            {
                MessageBox.Show("Hasło nieprawidłowe");
                Pas = "";
            }




        });

        private async Task AddSpec(Specialization item, ITransactionalApplicationData context)
        {
            await App.Current.Dispatcher.BeginInvoke((Action)(() => { context.Specializations.Add(item); }));

        }
        public void Initialize()
        {

            // var db = _applicationDataFactory.CreateApplicationData();
            if (_loggedUser.Logged is Patient)
            {
                _Patient = _loggedUser.Logged as Patient;
                _User = _Patient.User;
                Who = false;
            }
            else
            {
                _oldDoctor = _loggedUser.Logged as Doctor;
                _newDoctor = new Doctor();
                _newDoctor.CopyFrom(_oldDoctor);
                _User = _newDoctor.User;
                Who = true;
                var spec = _service.GetSpecializationsList();
                foreach (var VARIABLE in spec)
                {
                    if (VARIABLE.Key == _newDoctor.Specialization.Key)
                    {
                        Spec = VARIABLE;
                        break;
                    }
                }
                //Spec = us.Specialization;
                SpecList = spec;
                OnPropertyChanged("PS");
                OnPropertyChanged("PE");
                OnPropertyChanged("WS");
                OnPropertyChanged("WE");
                OnPropertyChanged("SS");
                OnPropertyChanged("SE");
                OnPropertyChanged("CS");
                OnPropertyChanged("CE");
                OnPropertyChanged("PIS");
                OnPropertyChanged("PIE");
                OnPropertyChanged("Spec");
            }
            OnPropertyChanged("Pesel");
            OnPropertyChanged("Who");
            OnPropertyChanged("FirstName");
            OnPropertyChanged("LastName");

        }
    }

}
