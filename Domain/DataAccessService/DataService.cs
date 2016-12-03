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
        public User GetUserByID(int id)
        {
            
            db.Fill();
            var a = db.Users.Find(id);
            return a;
        }

        public IEnumerable<Doctor> GetDoctorsList()
        {
            
            IEnumerable<Doctor> a = db.Doctors.Select(b=>b).Include(p=>p.User).Include(p=>p.Specialization).Include(p=>p.Visits);
            return a;
        }

        public IEnumerable<Specialization> GetSpecializationsList()
        {
            IEnumerable<Specialization> a = db.Specializations.Select(p => p).Include(p => p.Doctors);
            return a;
        }

        public IEnumerable<Visit> GetPatientVisits(int id)
        {
            IEnumerable<Visit> a = db.Visits.Select(p => p).Where(p=>p.Patient.Key==id).Include(p=>p.Doctor);
            return a;
        }
        public IEnumerable<Visit> GetDoctorVisits(int id)
        {
            IEnumerable<Visit> a = db.Visits.Select(p => p).Where(p => p.Doctor.Key == id).Include(p => p.Doctor);
            return a;
        }

        public bool UpdatePatient(Patient toUpdate)
        {
            return true;
        }

        public bool UpdateDoctor(Doctor toUpdate)
        {
            return true;
        }

        public bool DeleteDoctor(Doctor toDelete)
        {
            return true;
        }

        

        public bool DeletePatient(Patient toDelete)
        {
            return true;
        }

        public bool AddPatient(Patient toAdd)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            a.Patients.Add(toAdd);
            a.Commit();
            return true;
        }

        public bool AddDoctor(Doctor toAdd)
        {
            var a = new ApplicationDataFactory().CreateTransactionalApplicationData();
            Doctor d=new Doctor();
            d.User=new User();
            d.User.Kind=DocOrPat.Doctor;
            d.User.Name=new PersonName();
            d.User.Name.Name = "a";
            d.User.Name.Surname = "b";
            d.User.PESEL = "12341234123";
            d.User.Password="popaospaodpsa";
            d.Specialization=new Specialization("pupa");
            d.MondayWorkingTime=new WorkingTime();
            d.MondayWorkingTime.Start = 1;
            d.MondayWorkingTime.End = 2;
            d.TuesdayWorkingTime=new WorkingTime();
            d.TuesdayWorkingTime.Start = 1;
            d.TuesdayWorkingTime.End = 2;
            d.WednesdayWorkingTime = new WorkingTime();
            d.WednesdayWorkingTime.Start = 1;
            d.WednesdayWorkingTime.End=2; 
            d.ThursdayWorkingTime = new WorkingTime();
            d.ThursdayWorkingTime.Start = 1;
            d.ThursdayWorkingTime.End = 2;
            d.FridayWorkingTime = new WorkingTime();
            d.FridayWorkingTime.Start = 1;
            d.FridayWorkingTime.End = 2;

            a.Doctors.Add(d);
            a.Commit();
            return true;
        }
    }
}
