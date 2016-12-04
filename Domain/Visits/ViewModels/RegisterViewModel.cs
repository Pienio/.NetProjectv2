﻿using DatabaseAccess;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
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
using Visits.Utils;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Visits.ViewModels
{
    public class RegisterViewModel : ViewModel, IDataErrorInfo
    {
        private User _User;
        private IEnumerable<Specialization> _SpecList;
        private Patient _Patient = new Patient();
        private Doctor us = new Doctor();
        private string _pas = "";
        private string _pasp = "";
        private bool _Who;


        public RegisterViewModel(ILogUserService user) : base(user) { _Patient.User = new User(); }

        public bool Who
        {
            get { return _Who; }
            set
            {
                _Who = value;
                OnPropertyChanged("Who");

            }
        }


        public ICommand RegisterUser => new Command(async p =>
        {
            //var db = _applicationDataFactory.CreateTransactionalApplicationData();
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
            Person a;

            if (!_Who)
            {
                //IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == _Patient.User.PESEL);
                //if (asd.Count() != 0)
                //{
                //    MessageBox.Show("Istnieje juz użytkownik o takim peselu");
                //    return;
                //}


                await AddPatient(_Patient);
                // await 
                a = _Patient;
            }
            else
            {
                //IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == us.User.PESEL);
                //if (asd.Count() != 0)
                //{
                //    MessageBox.Show("Istnieje juz użytkownik o takim peselu");
                //    return;
                //}
                await AddDoctor(us);
                a = us;
            }

            //  db.Commit();
            _loggedUser.LogIn(a);


            Window k = p as Window;
            k.Close();


        });

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
                    if (string.IsNullOrWhiteSpace(FirstName))
                        result = "Imię nie może być puste!";
                }
                if (fieldName == "LastName")
                {
                    if (string.IsNullOrWhiteSpace(LastName))
                        result = "Nazwisko nie może być puste!";
                }
                if (fieldName == "Pesel")
                {
                    if (string.IsNullOrEmpty(Pesel))
                        result = "Pesel nie może być pusty!";
                    int a;

                    if (Int32.TryParse(Pesel, out a))
                        result = "Pesel musi być ciągiem cyfr!";
                    if (Pesel.Length != 11)
                        result = "Pesel musi mieć 11 cyfr!";


                }
                if (fieldName == "Pas")
                {

                    if (Pas.Length < 6)
                        result = "Hasło musi mieć 6 znaków!";

                }

                if (fieldName == "Pasp")
                {

                    if (Pas != null && Pas != Pasp)
                        result = "Hasła muszą być takie same!";
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
            set
            {
                _User.PESEL = value;
                OnPropertyChanged("Pesel");

            }
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
        public string Pasp
        {
            get { return _pasp; }
            set
            {
                _pasp = value;
                _User.Password = PasswordHasher.CreateHash(value);
                OnPropertyChanged("Pasp");
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
            get { return us.Specialization; }
            set
            {
                us.Specialization = value;
                OnPropertyChanged("Spec");
            }
        }

        public int PS
        {
            get { return us.MondayWorkingTime.Start; }
            set
            {

                us.MondayWorkingTime.Start = value;
                OnPropertyChanged("PS");
            }
        }
        public int PE
        {
            get { return us.MondayWorkingTime.End; }
            set
            {
                us.MondayWorkingTime.End = value;
                OnPropertyChanged("PE");
            }
        }
        public int WS
        {
            get { return us.TuesdayWorkingTime.Start; }
            set
            {
                us.TuesdayWorkingTime.Start = value;
                OnPropertyChanged("WS");
            }
        }
        public int WE
        {
            get { return us.TuesdayWorkingTime.End; }
            set
            {
                us.TuesdayWorkingTime.End = value;
                OnPropertyChanged("WE");
            }
        }
        public int SS
        {
            get { return us.WednesdayWorkingTime.Start; }
            set
            {
                us.WednesdayWorkingTime.Start = value;
                OnPropertyChanged("SS");
            }
        }
        public int SE
        {
            get { return us.WednesdayWorkingTime.End; }
            set
            {
                us.WednesdayWorkingTime.End = value;
                OnPropertyChanged("SE");
            }
        }
        public int CS
        {
            get { return us.ThursdayWorkingTime.Start; }
            set
            {
                us.ThursdayWorkingTime.Start = value;
                OnPropertyChanged("CS");
            }
        }
        public int CE
        {
            get { return us.ThursdayWorkingTime.End; }
            set
            {
                us.ThursdayWorkingTime.End = value;
                OnPropertyChanged("CE");
            }
        }
        public int PIS
        {
            get { return us.FridayWorkingTime.Start; }
            set
            {
                us.FridayWorkingTime.Start = value;
                OnPropertyChanged("PIS");
            }
        }
        public int PIE
        {
            get { return us.FridayWorkingTime.End; }
            set
            {
                us.FridayWorkingTime.End = value;
                OnPropertyChanged("PIE");
            }
        }
        public async Task AddPatient(Patient item)
        {
            var c = await Task.Run(() => _service.AddPatient(item));
            if (!c)
                MessageBox.Show("Istnieje juz użytkownik o takim peselu");
        }
        public async Task AddDoctor(Doctor item)
        {
            var c = await Task.Run(() => _service.AddRequest(new ProfileRequest(null, item)));
            if (!c)
                MessageBox.Show("Istnieje juz użytkownik o takim peselu");
            // await App.Current.Dispatcher.BeginInvoke((Action)(() => { _service.AddDoctor(item); }));

        }

        public ICommand ChangePass => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pas = a.Password;
            OnPropertyChanged("Pasp");
        });
        public ICommand Close => new Command(p =>
        {

            Window k = p as Window;
            k.Close();

        });
        public ICommand ChangePass1 => new Command(p =>
        {

            PasswordBox a = (PasswordBox)p;
            Pasp = a.Password;


        });
        public void Initialize(bool Wh)
        {
            _User = new User();
            _User.Name = new PersonName();
            _Who = Wh;

            if (!Wh)
            {
                _Patient = new Patient();
                _Patient.User = _User;
                _Patient.User.Kind = DocOrPat.Patient;

                OnPropertyChanged("FirstName");
                OnPropertyChanged("LastName");
            }
            else
            {
                us = new Doctor();
                us.User = _User;
                us.MondayWorkingTime = new WorkingTime();
                us.TuesdayWorkingTime = new WorkingTime();
                us.ThursdayWorkingTime = new WorkingTime();
                us.WednesdayWorkingTime = new WorkingTime();
                us.FridayWorkingTime = new WorkingTime();
                us.Specialization = new Specialization();
                us.User.Kind = DocOrPat.Doctor;

                // var db = _applicationDataFactory.CreateApplicationData();

                OnPropertyChanged("FirstName");
                OnPropertyChanged("LastName");
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
                Load();

            }
            Pesel = "";
            OnPropertyChanged("Who");

        }
        public void Load()
        {
            // var db = _applicationDataFactory.CreateApplicationData();

            // var spec = new List<Specialization>();

            //  spec.AddRange(db.Specializations);
            SpecList = _service.GetSpecializationsList();
            Spec = SpecList.First();
            //// OnPropertyChanged(nameof(SpecList));
        }

    }
}
