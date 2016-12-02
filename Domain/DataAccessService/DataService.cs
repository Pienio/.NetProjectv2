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
        public User GetUserByID(int id)
        {
            IApplicationData db = new ApplicationDataFactory().CreateApplicationData();
            db.Fill();
            var a = db.Users.Find(id);
            return a;
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            IApplicationData db = new ApplicationDataFactory().CreateApplicationData();
            //db.Fill();
            
             db.Doctors.Load();
            var c = db.Doctors.Local;
         
            return c;
            //IEnumerable<Doctor> a = db.Doctors.Select(b=>b);
            //return a;
        }
    }
}
