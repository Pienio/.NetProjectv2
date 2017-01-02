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
            var b = a.FirstOrDefault();
            return b;
        }

        public Doctor GetDoctorById(int value)
        {
            var a = db.Doctors.Select(p=>p).Where(p=>p.Key==value).Include(p=>p.User).Include(p=>p.Specialization).Include(p=>p.Visits);
            Doctor c = null;
            if(a!=null)
                c = a.First();
            
            return c;
        }

        public Doctor GetDoctorByUserId(int value)
        {
            var a = db.Doctors.Select(p => p).Where(p => p.User.Key == value).Include(p => p.User).Include(p=>p.Visits).Include(p=>p.Specialization).Include(p => p.Visits);
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
            IEnumerable<Doctor> a = db.Doctors.Select(b => b).Where(p=>p.ProfileAccepted).Include(p => p.User).Include(p => p.Specialization).Include(p => p.Visits);
            return a;
        }

        public IEnumerable<Doctor> SearchDoctorsList(Specialization spec, string name)
        {
            IEnumerable<Doctor> w;
            string namez = name?.ToLower();
           

            if (spec == null)
            {
                List<Doctor> a = new List<Doctor>();
                w = db.Doctors.Select(b => b).Where(p => p.ProfileAccepted && p.User.Active).Include(p => p.User).Include(p => p.Specialization).Include(p => p.Visits);
                if (!string.IsNullOrWhiteSpace(namez))
                {
                    foreach (var VARIABLE in w)
                    {
                        if (VARIABLE.User.Name.ToString().ToLower().Contains(namez))
                            a.Add(VARIABLE);
                    }
                    w = a;
                }
            }
            else
            {
                w = db.Doctors.Select(b => b).Where(p => p.ProfileAccepted && p.User.Active).Include(p => p.User).Include(p => p.Specialization).Include(p => p.Visits);
                List<Doctor> a=new List<Doctor>();
                if (!string.IsNullOrWhiteSpace(namez))
                {
                    foreach (var VARIABLE in w)
                    {
                        bool flag = false;
                        foreach (var s in VARIABLE.Specialization)
                        {
                            if (s.Name == spec.Name)
                                flag = true;
                        }
                      
                        if (flag && VARIABLE.User.Name.ToString().ToLower().Contains(namez))
                            a.Add(VARIABLE);
                    }
                }
                else
                {
                    foreach (var VARIABLE in w)
                    {
                        bool flag = false;
                        foreach (var s in VARIABLE.Specialization)
                        {
                            if (s.Name == spec.Name)
                                flag = true;
                        }
                        if (flag)
                            a.Add(VARIABLE);
                    }
                }
               
                w = a;
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
                    .Include(p => p.Doctor).Include(p=>p.Patient).Include(p => p.Patient.User).Include(p => p.Doctor.User);
            return a;
        }
        public IEnumerable<Visit> GetDoctorVisits(int id, bool tr)
        {
            DateTime now=DateTime.Now;
            IEnumerable<Visit> a =
              db.Visits.Select(p => p)
                  .Where(p => p.Doctor.Key == id && (tr ? p.Date <= now : p.Date > now))
                  .Include(p => p.Patient).Include(p=>p.Doctor).Include(p=>p.Patient.User).Include(p => p.Doctor.User);
            return a;
        }
        public IEnumerable<ProfileRequest> GetRequests()
        {
            return db.Requests.Include(r => r.OldProfile).Include(r => r.NewProfile).Include(p=>p.NewProfile.User).Include(p=>p.NewProfile.Specialization);
        }

        public bool UpdatePatient(Patient toUpdate)
        {
            db.BeginTransaction();
            var o = db.Patients.Select(p=>p).Where(p=>p.Key==toUpdate.Key).Include(p=>p.User).Include(p=>p.Visits).First();
            o.User.Name = toUpdate.User.Name;
            o.User.Mail = toUpdate.User.Mail;
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
            var c = db.Users.Select(p=>p).Where(p=>p.Key==id).First();
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
            var o = db.Doctors.Select(p=>p).Where(p=>p.Key==toUpdate.Key).Include(p=>p.User).Include(p=>p.Specialization).Include(p=>p.Visits).First();
           // toUpdate.Specialization.Doctors.Add(toUpdate);
            o.User.Name = toUpdate.User.Name;
            o.User.Mail = toUpdate.User.Mail;
            o.ProfileAccepted = toUpdate.ProfileAccepted;
            o.User.PESEL = toUpdate.User.PESEL;
            o.User.Password = toUpdate.User.Password;
            o.Specialization = new List<Specialization>();
            List<Specialization> nn = new List<Specialization>();
            o.FirstFreeSlot = DateTime.Now;
            o.MondayWorkingTime = toUpdate.MondayWorkingTime;
            o.WednesdayWorkingTime = toUpdate.WednesdayWorkingTime;
            o.FridayWorkingTime = toUpdate.FridayWorkingTime;
            o.TuesdayWorkingTime = toUpdate.TuesdayWorkingTime;
            o.ThursdayWorkingTime = toUpdate.ThursdayWorkingTime;

            o.Visits = toUpdate.Visits;
            foreach (var VARIABLE in toUpdate.Specialization)
            {
                Specialization asdf = db.Specializations.Select(p => p).Where(p => p.Name == VARIABLE.Name).Include(p=>p.Doctors).First();
                if(!asdf.Doctors.Any(p=>p.Key==toUpdate.Key))
                    asdf.Doctors.Add(o);
                nn.Add(asdf);
            }
            o.Specialization = nn;
            //o.Specialization = toUpdate.Specialization;
         
            
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
            // Patient asd = _loggedUser.Logged as Patient;
            var b = db.Doctors.Select(p=>p).Where(p=>p.Key==toDelete.Key).Include(p=>p.User).Include(p=>p.Specialization).Include(p=>p.Visits).First();
            IEnumerable<Visit> obw = GetDoctorVisits((int)b.Key, true);
            if (obw == null || obw.ToList().Count == 0)
            {
                User adfg = b.User;
                db.Users.Attach(adfg);
                db.Users.Remove(adfg);
                db.Doctors.Attach(b);
                db.Doctors.Remove(b);
              
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
            var b = db.Patients.Select(p=>p).Where(p=>p.Key==toDelete.Key).Include(p=>p.User).Include(p=>p.Visits).First();
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
            var c = db.Specializations.Select(p => p).Where(p => p.Name == toDelete.Name).First();
            
            //if (db.Doctors.Any(d => d.Specialization.Contains(c)))
            //    return false;
             //db.Specializations.Attach(toDelete);
            db.Specializations.Remove(c);
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
            //db.Requests.Attach(toDelete);
            if (toDelete.OldProfile != null)
            {
                Doctor a = GetDoctorById((int)toDelete.NewProfile.Key);
                User b = db.Users.Find(a.User.Key);
                db.Doctors.Remove(a);
                db.Users.Remove(b);
            }
               
            ProfileRequest td =
                db.Requests.Select(p => p)
                    .Where(p => p.Key == toDelete.Key)
                    .Include(p => p.OldProfile)
                    .Include(p => p.NewProfile)
                    .First();
            db.Requests.Remove(td);
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
            
            Patient c = new Patient();
            c.User = new User();
            c.User.Name = new PersonName();
            c.User.Kind = DocOrPat.Patient;
             c.User.Name = toAdd.User.Name;
         
            c.User.PESEL = toAdd.User.PESEL;
            c.User.Password = toAdd.User.Password;
          
            

            db.Patients.Add(toAdd);
            db.Commit();
            return true;
        }

        public bool AddDoctor(Doctor toAdd)
        {
            db.BeginTransaction();
            IEnumerable<User> asd = db.Users.Select(d => d).Where(d => d.PESEL == toAdd.User.PESEL);
            if (asd==null||asd.Count() != 0)
            {
                return false;
            }
           
           List<Specialization> nn=new List<Specialization>();
            foreach (var VARIABLE in toAdd.Specialization)
            {
                Specialization asdf = db.Specializations.Select(p => p).Where(p => p.Name == VARIABLE.Name).Include(p => p.Doctors).First();
                asdf.Doctors.Add(toAdd);
                nn.Add(asdf);
            }
            toAdd.Specialization = nn;
            toAdd.FirstFreeSlot=DateTime.Now;
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
       
            toAdd.NewProfile.FirstFreeSlot=DateTime.Now;
            List<Specialization> nn = new List<Specialization>();
            foreach (var VARIABLE in toAdd.NewProfile.Specialization)
            {
                Specialization asdf = db.Specializations.Select(p => p).Where(p => p.Name == VARIABLE.Name).Include(p => p.Doctors).First();
                asdf.Doctors.Add(toAdd.NewProfile);
                nn.Add(asdf);
            }
            toAdd.NewProfile.Specialization = nn;
            db.Doctors.Add(toAdd.NewProfile);
            toAdd.NewProfile = db.Doctors.Find(toAdd.NewProfile.Key);
            if (toAdd.OldProfile != null)
                toAdd.OldProfile = db.Doctors.Find(toAdd.OldProfile.Key);
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

        public bool DeleteVisit(Visit ToDelete)
        {
            var a =
                db.Visits.Select( p =>p).Where(p => p.Date == ToDelete.Date && p.Doctor.Key == ToDelete.Doctor.Key &&
                            p.Patient.Key == ToDelete.Patient.Key).First();
            db.BeginTransaction();
            db.Visits.Remove(a);

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
