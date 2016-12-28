using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.Model;

namespace DataAccessService
{
    public class DataService : IDataService
    {
        ITransactionalApplicationData db = new ApplicationDataFactory().CreateTransactionalApplicationData(false);
        public User GetUserById(int id)
        {
            db.Fill();
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
            var a = db.Doctors.Select(p=>p).Where(p=>p.Key==value).Include(p=>p.User).Include(p=>p.Specialization);
            Doctor c = null;
            if(a!=null)
                c = a.First();
            
            return c;
        }

        public Doctor GetDoctorByUserId(int value)
        {
            var a = db.Doctors.Select(p => p).Where(p => p.User.Key == value).Include(p => p.User).Include(p=>p.Visits).Include(p=>p.Specialization);
            Doctor c = null;
            if (a != null)
                c = a.First();

            return c;
        }

        public Patient GetPatientById(int value)
        {
            var a = db.Patients.Select(p => p).Where(p => p.Key == value).Include(p=>p.User);
            Patient c = null;
            if (a != null)
                c = a.First();
            return c;
        }

        public Patient GetPatientByUserId(int value)
        {
            var a = db.Patients.Select(p => p).Where(p => p.User.Key == value).Include(p => p.User);
            Patient c = null;
            if (a != null)
                c = a.First();
            return c;
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
                w = db.Doctors.Select(p => p).Where(p => p.Specialization.Key == spec.Key && p.User.Active);//.Include(p=>p.User).Include(p=>p.Visits).Include(p=>p.Specialization);
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
            IEnumerable<Specialization> a = db.Specializations;//.Include(p => p.Doctors);
            return a;
        }

        public IEnumerable<Visit> GetPatientVisits(int id, bool tr)
        {
            DateTime now = DateTime.Now;
            IEnumerable<Visit> a =
                db.Visits.Select(p => p)
                    .Where(p => p.Patient.Key == id && (tr ? p.Date <= now : p.Date > now))
                    .Include(p => p.Doctor);
            return a;
        }
        public IEnumerable<Visit> GetDoctorVisits(int id, bool tr)
        {
            DateTime now=DateTime.Now;
            IEnumerable<Visit> a =
              db.Visits.Select(p => p)
                  .Where(p => p.Doctor.Key == id && (tr ? p.Date <= now : p.Date > now))
                  .Include(p => p.Patient);
            return a;
        }
        public IEnumerable<ProfileRequest> GetRequests()
        {
            return db.Requests.Include(r => r.OldProfile).Include(r => r.NewProfile);
        }

        public bool UpdatePatient(Patient toUpdate)
        {
            db.BeginTransaction();
            var o = db.Patients.Find(toUpdate.Key);
            o.User.Name = toUpdate.User.Name;
            o.User.PESEL = toUpdate.User.PESEL;
            o.User.Password = toUpdate.User.Password;
            o.Visits = toUpdate.Visits;

            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateUserPassword(int id, string pass)
        {
            db.BeginTransaction();
            var c = db.Users.Find(id);
            c.Password = pass;
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateDoctor(Doctor toUpdate)
        {
            db.BeginTransaction();
            var o = db.Doctors.Find(toUpdate.Key);
           // toUpdate.Specialization.Doctors.Add(toUpdate);
            o.User.Name = toUpdate.User.Name;
            o.User.PESEL = toUpdate.User.PESEL;
            o.User.Password = toUpdate.User.Password;
            o.Specialization = db.Specializations.Find(toUpdate.Specialization.Key);
            o.MondayWorkingTime = toUpdate.MondayWorkingTime;
            o.WednesdayWorkingTime = toUpdate.WednesdayWorkingTime;
            o.FridayWorkingTime = toUpdate.FridayWorkingTime;
            o.TuesdayWorkingTime = toUpdate.TuesdayWorkingTime;
            o.ThursdayWorkingTime = toUpdate.ThursdayWorkingTime;
            o.Visits = toUpdate.Visits;
            
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateSpecialization(Specialization toUpdate)
        {
            db.BeginTransaction();
            var spec = db.Specializations.Find(toUpdate.Key);
            spec.Name = toUpdate.Name;
            try
            {
                db.Commit();
                return true;
        }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteDoctor(Doctor toDelete)
        {
            db.BeginTransaction();
            var b = db.Doctors.Find(toDelete.Key);
            IEnumerable<Visit> obw = GetDoctorVisits((int)b.Key, true);
            if (obw == null || obw.ToList().Count == 0)
            {
                User adfg = b.User;
                db.Doctors.Attach(b);
                db.Doctors.Remove(b);
                db.Users.Attach(adfg);
                db.Users.Remove(adfg);
            }
            else
            {
                for (int i = 0; i < toDelete.Visits.Count; i++)
                {
                    if (b.Visits[i].Date > DateTime.Now)
                    {
                        db.Visits.Attach(b.Visits[i]);
                        db.Visits.Remove(b.Visits[i]);

                    }
                }
                b.User.Active = false;
            }
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        

        public bool DeletePatient(Patient toDelete)
        {
            db.BeginTransaction();
            var b = db.Patients.Find(toDelete.Key);
            IEnumerable<Visit> obw = GetPatientVisits((int)b.Key, true);
            if (obw == null || obw.ToList().Count == 0)
            {
                User adfg = b.User;
                db.Patients.Attach(b);
                db.Patients.Remove(b);
                db.Users.Attach(adfg);
                db.Users.Remove(adfg);
            }
            else
            {
                for (int i = 0; i < toDelete.Visits.Count; i++)
                {
                    if (b.Visits[i].Date > DateTime.Now)
                    {
                        db.Visits.Attach(b.Visits[i]);
                        db.Visits.Remove(b.Visits[i]);

                    }
                }
                b.User.Active = false;
            }
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //return true;
        }

        public bool DeleteSpecialization(Specialization toDelete)
        {
            db.BeginTransaction();
            if (db.Doctors.Any(d => d.Specialization.Key == toDelete.Key))
                return false;
            db.Specializations.Attach(toDelete);
            db.Specializations.Remove(toDelete);
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteRequest(ProfileRequest toDelete)
        {
            db.BeginTransaction();
            db.Requests.Attach(toDelete);
            db.Requests.Remove(toDelete);
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddPatient(Patient toAdd)
        {
            db.BeginTransaction();
            //IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == toAdd.User.PESEL);
            //if (asd.Count() != 0)
            //{
            //    return false;
            //    // MessageBox.Show("Istnieje juz użytkownik o takim peselu");
            //    // return;
            //}
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
            

            db.Patients.Add(toAdd);
            db.Commit();
            return true;
        }

        public bool AddDoctor(Doctor toAdd)
        {
            db.BeginTransaction();
            IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == toAdd.User.PESEL);
            if (asd.Count() != 0)
            {
                return false;
            }
            toAdd.Specialization = this.db.Specializations.Find(toAdd.Specialization.Key);
            
            db.Doctors.Add(toAdd);
            db.Commit();
            return true;
        }

        public bool AddSpecialization(Specialization toAdd)
        {
            db.BeginTransaction();
            db.Specializations.Add(toAdd);
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddRequest(ProfileRequest toAdd)
        {
            db.BeginTransaction();
            if (toAdd.OldProfile == null && db.Users.Any(d => d.PESEL == toAdd.NewProfile.User.PESEL))
                return false;
            if (toAdd.OldProfile != null)
                if (db.Requests.Any(r => r.NewProfile.Key == toAdd.NewProfile.Key || r.NewProfile.User.Key == toAdd.NewProfile.User.Key))
                    return false;   
            db.Requests.Add(toAdd);
                db.Commit();
                return true;
            }

        public bool RegisterVisit(DateTime selected, int patientId, int doctorId)
        {
            var a = db.Patients.Select(p=>p).Where(p=>p.Key==patientId).Include(p=>p.User).Include(p=>p.Visits).First();
            var b = db.Doctors.Select(p => p).Where(p => p.Key == doctorId).Include(p => p.User).Include(p=>p.Specialization).Include(p => p.Visits).First();
            b.FirstFreeSlot = GetFirstFreeSlot((int)b.Key);
            db.BeginTransaction();
            var c = new Visit(a, b, selected);
            a.Visits.Add(c);
            b.Visits.Add(c);
            db.Visits.Add(c);
            
            try
            {
                db.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DateTime GetFirstFreeSlot(int doctorId)
        {
            Doctor doc = db.Doctors.Find(doctorId);
            return doc.GetFirstFreeSlot();
        }

        public IEnumerable<Patient> GetPatients()
        {
            IEnumerable<Patient> a = db.Patients.Select(p => p).Include(p => p.User).Include(p => p.Visits);
            return a;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
