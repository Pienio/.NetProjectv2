using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseAccess.Model;

namespace UnitTestProject1
{
    [TestClass]
    public class DataServiceTests
    {
        [TestMethod]
        public void GetDoctor()
        {
            var a = TestingExtension.GetService();
            Random r = new Random();
            int d = r.Next(2, 10);
            var doc = a.GetDoctorById(d);

            if (d > 9)
            {
                if (doc != null)
                    Assert.Fail();

            }
            else
            {
                if (doc == null)
                    Assert.Fail();
            }

            var doc1 = a.GetDoctorByUserId(d);

            if (d > 9)
            {
                if (doc1 != null)
                    Assert.Fail();

            }
            else
            {
                if (doc1 == null)
                    Assert.Fail();
            }

        }
        [TestMethod]
        public void GetUser()
        {

            string[] pesels = { "09586749381", "19683750923", "94860285691", "58672349682", "38596827364", "58476923857", "88975643287", "29384795618" };
            string password = "96e79218965eb72c92a549dd5a330112";

            var a = TestingExtension.GetService();
            Random r = new Random();
            int d = r.Next(2, 10);
            var doc = a.GetUserById(d);

            if (d > 9)
            {
                if (doc != null)
                    Assert.Fail();

            }
            else
            {
                if (doc == null)
                    Assert.Fail();
            }

            

            if (d <= 8)
            {
                doc = a.GetUser(pesels[d], password);
                if (doc == null)
                    Assert.Fail();

            }
         



        }
        [TestMethod]
        public void GetPatient()
        {


            var a = TestingExtension.GetService();
            Random r = new Random();
            int d = r.Next(1,2);
            var doc = a.GetUserById(d);

            if(d==1&&doc==null)
                Assert.Fail();

        }
        [TestMethod]
        public void GetDoctorsList()
        {


            var a = TestingExtension.GetService();

            var doc = a.GetDoctorsList();

            if (doc == null)
                Assert.Fail();

        }
        [TestMethod]
        public void SearchDoctors()
        {

            Specialization[] specs = {
            new Specialization("Reumatolog"),
            new Specialization("Kardiolog"),
            new Specialization("Neurolog"),
            new Specialization("Urolog"),
            new Specialization("Okulista"),
            new Specialization("Psychiatra"),
            new Specialization("Ginekolog"),
            new Specialization("Pediatra")};
            string[] names = { "Kuba", "Jan", "Łukasz", "Adrian", "Bartosz", "Marek", "Filip", "Bartłomiej" };
            string[] surnames = { "Soczkowski", "Berwid", "Okular", "Michałowski", "Skała", "Mikowski", "Wasiłkowski", "Normowski" };
            var a = TestingExtension.GetService();
            Random r = new Random();
            int d = r.Next(0, 7);
            Specialization n = new Specialization();

            n.Name = specs[d].Name;



            var doc = a.SearchDoctorsList(n, names[d] + " " + surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(null, names[d] + " " + surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(n,surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(n, names[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(null, surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(n, surnames[d/2]);
            if (doc != null)
                Assert.Fail();

        }
        [TestMethod]
        public void GetSpecList()
        {


            var a = TestingExtension.GetService();

            var doc = a.GetSpecializationsList();

            if (doc == null)
                Assert.Fail();

        }
        [TestMethod]
        public void CheckAddandDeleteDoctor()
        {


            var a = TestingExtension.GetService();

            var doc = TestingExtension.GetDoctor();

            a.AddDoctor(doc);
            var c = a.SearchDoctorsList(doc.Specialization[0], doc.User.Name.ToString());
            if(c==null)
                Assert.Fail();

            var dd=a.DeleteDoctor(c.First());
            Assert.IsTrue(dd);




        }
        [TestMethod]
        public void CheckAddandDeletePatient()
        {


            var a = TestingExtension.GetService();

            var doc = TestingExtension.GetPatient();

            a.AddPatient(doc);
            var c = a.GetUser(doc.User.PESEL,doc.User.Password);
            var d = a.GetPatientByUserId((int)c.Key);
            if (d == null)
                Assert.Fail();

            var dd = a.DeletePatient(d);
            Assert.IsTrue(dd);




        }

        [TestMethod]
        public void CheckAddandDeleteVisit()
        {


            var a = TestingExtension.GetService();

            var pac = TestingExtension.GetPatient();
            var doc = TestingExtension.GetDoctor();

            a.AddPatient(pac);
            a.AddDoctor(doc);
            var c = a.GetUser(doc.User.PESEL, doc.User.Password);
            pac = a.GetPatientByUserId((int)c.Key);
            doc = a.SearchDoctorsList(doc.Specialization[0], doc.User.Name.ToString()).First();
            var  date= a.GetFirstFreeSlot((int) doc.Key);

            bool dd=a.RegisterVisit(date, (int) pac.Key, (int) doc.Key);
            Assert.IsTrue(dd);
            var pacviz = a.GetPatientVisits((int)pac.Key, false);
            var docviz = a.GetDoctorVisits((int) doc.Key, false);
            if(pacviz.Length==0||docviz.Length==0)
                Assert.Fail();

            dd=a.DeleteVisit(pacviz.First());
            Assert.IsTrue(dd);

            dd = a.DeletePatient(pac);
            Assert.IsTrue(dd);
            dd = a.DeleteDoctor(doc);
            Assert.IsTrue(dd);


        }

        [TestMethod]
        public void CheckAddandDeleteSpec()
        {


            var a = TestingExtension.GetService();
            Specialization nn= new Specialization();
            nn.Name = "dddddd";
            a.AddSpecialization(nn);
            var list = a.GetSpecializationsList();
            var dd = list.ToList();
            var added = dd.Find(p => p.Name == nn.Name);
            if(added==null)
                Assert.Fail();

            bool flag = a.DeleteSpecialization(added);
            Assert.IsTrue(flag);





        }
    }
}
