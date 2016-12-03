using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DatabaseAccess;
using DatabaseAccess.Model;

namespace DataAccessService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
   
    public class DataService : IDataService
    {
        IApplicationData db = new ApplicationDataFactory().CreateApplicationData();
        public User GetUserById(int id)
        {

           // db.Fill();
            var a = db.Users.Find(id);
            return a;
        }

        public User GetUser(string pes, string password)
        {
            var a = db.Users.Select(p => p).Where(p => p.PESEL == pes && p.Password == password && p.Active);
            var b = a.First();
            return b;
        }

        public Doctor GetDoctorById(int value)
        {
            var a = db.Doctors.Select(p=>p).Where(p=>p.User.Key==value);
            var c = a.First();
            
            return c;
        }

        public Patient GetPatientById(int value)
        {
            var a = db.Patients.Select(p => p).Where(p => p.User.Key == value);
            return a.First();
        }

        public void Fill()
        {
            db.Fill();
        }

        public IEnumerable<Doctor> GetDoctorsList()
        {
            //return null;
            IEnumerable<Doctor> a = db.Doctors.Select(b => b).Include(p => p.User).Include(p => p.Specialization).Include(p => p.Visits);
            return a;
        }

        public IEnumerable<Doctor> SearchDoctorsList(Specialization spec, string name)
        {
            IEnumerable<Doctor> w;
            string namez = name?.ToLower();
            if (spec == null)
                w = db.Doctors.Select(p => p); //.Include(p => p.User).Include(p => p.Visits).Where(p=>p.User.Active);
            else
                w = db.Doctors.Select(p => p).Where(p => p.Specialization.Key == spec.Key&&p.User.Active);//.Include(p=>p.User).Include(p=>p.Visits).Include(p=>p.Specialization);
            if (string.IsNullOrWhiteSpace(namez))
            {
                w = w.Select(p => p).Where(p => p.User.Active);

            }
            else
            {
                w = w.Select(p => p).Where(p => p.User.Name.ToString().ToLower().Contains(namez) && p.User.Active);
            }
            return w;
        }

        public IEnumerable<Specialization> GetSpecializationsList()
        {
            IEnumerable<Specialization> a = db.Specializations.Select(p => p);//.Include(p => p.Doctors);
            return a;
        }

        public IEnumerable<Visit> GetPatientVisits(int id,bool tr)
        {
            //IEnumerable<Visit> a = db.Visits.Select(p => p).Where(p=>p.Patient.Key==id).Include(p=>p.Doctor);
            DateTime now = DateTime.Now;
            IEnumerable<Visit> a = from v in db.Visits.Local
                                   where v.Patient.Key == id && (tr ? v.Date <= now : v.Date > now)
                                   select v;
            return a;
        }
        public IEnumerable<Visit> GetDoctorVisits(int id,bool tr)
        {
            DateTime now=DateTime.Now;
            IEnumerable<Visit> a = from v in db.Visits
                where v.Doctor.Key == id && (tr ? v.Date <= now : v.Date > now)
                select v;//db.Visits.Select(p => p).Where(p => p.Doctor.Key == id).Include(p => p.Doctor);
            return a;
        }

        public bool UpdatePatient(Patient toUpdate)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            var o = a.Patients.Find(toUpdate.Key);
            o.User.Name = toUpdate.User.Name;
            o.User.PESEL = toUpdate.User.PESEL;
            o.User.Password = toUpdate.User.Password;
            o.Visits = toUpdate.Visits;

            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateUserPassword(int id, string pass)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            var c = a.Users.Find(id);
            c.Password = pass;
            a.Commit();
            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateDoctor(Doctor toUpdate)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            var o = a.Doctors.Find(toUpdate.Key);
           // toUpdate.Specialization.Doctors.Add(toUpdate);
            o.User.Name = toUpdate.User.Name;
            o.User.PESEL = toUpdate.User.PESEL;
            o.User.Password = toUpdate.User.Password;
            o.Specialization = a.Specializations.Find(toUpdate.Specialization.Key);
            o.MondayWorkingTime = toUpdate.MondayWorkingTime;
            o.WednesdayWorkingTime = toUpdate.WednesdayWorkingTime;
            o.FridayWorkingTime = toUpdate.FridayWorkingTime;
            o.TuesdayWorkingTime = toUpdate.TuesdayWorkingTime;
            o.ThursdayWorkingTime = toUpdate.ThursdayWorkingTime;
            o.Visits = toUpdate.Visits;
            
            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            
        }

        public bool DeleteDoctor(Doctor toDelete)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            // Patient asd = _loggedUser.Logged as Patient;
            var b = a.Doctors.Find(toDelete.Key);
            IEnumerable<Visit> obw = GetDoctorVisits((int)b.Key, true);
            if (obw==null||obw.ToList().Count == 0)
            {
                User adfg = b.User;
                a.Doctors.Attach(b);
                a.Doctors.Remove(b);
                a.Users.Attach(adfg);
                a.Users.Remove(adfg);
            }
            else
            {
                for (int i = 0; i < toDelete.Visits.Count; i++)
                {
                    if (b.Visits[i].Date > DateTime.Now)
                    {
                        a.Visits.Attach(b.Visits[i]);
                        a.Visits.Remove(b.Visits[i]);

                    }
                }
                b.User.Active = false;
            }
            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        

        public bool DeletePatient(Patient toDelete)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
           // Patient asd = _loggedUser.Logged as Patient;
            var b = a.Patients.Find(toDelete.Key);
            IEnumerable<Visit> obw = GetPatientVisits((int)b.Key, true);
            if (obw==null||obw.ToList().Count == 0)
            {
                User adfg = b.User;
                a.Patients.Attach(b);
                a.Patients.Remove(b);
                a.Users.Attach(adfg);
                a.Users.Remove(adfg);
            }
            else
            {
                for (int i = 0; i < toDelete.Visits.Count; i++)
                {
                    if (b.Visits[i].Date > DateTime.Now)
                    {
                        a.Visits.Attach(b.Visits[i]);
                        a.Visits.Remove(b.Visits[i]);

                    }
                }
                b.User.Active = false;
            }
            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //return true;
        }

        public bool AddPatient(Patient toAdd)
        {
            //IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == toAdd.User.PESEL);
            //if (asd.Count() != 0)
            //{
            //    return false;
            //    // MessageBox.Show("Istnieje juz użytkownik o takim peselu");
            //    // return;
            //}
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            Patient c = new Patient();
            c.User = new User();
            c.User.Name = new PersonName();
            c.User.Kind = DocOrPat.Patient;
             c.User.Name = toAdd.User.Name;
           // c.User.Name.Name = "asd";
          //  c.User.Name.Surname = "aas";
            c.User.PESEL = toAdd.User.PESEL;
            c.User.Password = toAdd.User.Password;
           // c.Visits=new List<Visit>();
            

            a.Patients.Add(toAdd);
            a.Commit();
            return true;
        }

        public bool AddDoctor(Doctor toAdd)
        {
            IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == toAdd.User.PESEL);
            if (asd.Count() != 0)
            {
                return false;
                // MessageBox.Show("Istnieje juz użytkownik o takim peselu");
                // return;
            }
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            //Doctor d = new Doctor();
            //d.User = new User();
            //d.User.Kind = DocOrPat.Doctor;
            //d.User.Name = new PersonName();
            //d.User.Name.Name = "a";
            //d.User.Name.Surname = "b";
            //d.User.PESEL = "12341234123";
            //d.User.Password = "popaospaodpsa";
            //d.Specialization = new Specialization("pupa");
            //d.MondayWorkingTime = new WorkingTime();
            //d.MondayWorkingTime.Start = 1;
            //d.MondayWorkingTime.End = 2;
            //d.TuesdayWorkingTime = new WorkingTime();
            //d.TuesdayWorkingTime.Start = 1;
            //d.TuesdayWorkingTime.End = 2;
            //d.WednesdayWorkingTime = new WorkingTime();
            //d.WednesdayWorkingTime.Start = 1;
            //d.WednesdayWorkingTime.End = 2;
            //d.ThursdayWorkingTime = new WorkingTime();
            //d.ThursdayWorkingTime.Start = 1;
            //d.ThursdayWorkingTime.End = 2;
            //d.FridayWorkingTime = new WorkingTime();
            //d.FridayWorkingTime.Start = 1;
            //d.FridayWorkingTime.End = 2;
            toAdd.Specialization = db.Specializations.Find(toAdd.Specialization.Key);
            
            a.Doctors.Add(toAdd);
            a.Commit();
            return true;
        }

        public bool AddSpecialization(Specialization toAdd)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            a.Specializations.Add(toAdd);
            try
            {
                a.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RegisterVisit(DateTime selected, int patientId, int doctorId)
        {
            throw new NotImplementedException();
        }
    }
}
