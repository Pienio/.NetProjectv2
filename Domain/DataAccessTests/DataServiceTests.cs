﻿using System;
using System.Collections.Generic;
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
            int d = r.Next(1, 10);
            var doc = a.GetDoctorById(d);

            if (d > 8)
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

            if (d > 8)
            {
                if (doc1 != null||doc1.User.Kind==DocOrPat.Patient)
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
            int d = r.Next(1, 10);
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

            

            if (d <= 7)
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
      


            var doc = a.SearchDoctorsList(specs[d], names[d] + " " + surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(null, names[d] + " " + surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(specs[d], surnames[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(specs[d], names[d]);
            if (doc == null)
                Assert.Fail();
            doc = a.SearchDoctorsList(null, surnames[d]);
            if (doc == null)
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
             var cer = a.GetSpecializationsList();
            var spec = cer[0];
            doc.Specialization = new List<Specialization>();
            doc.Specialization.Add(spec);
            doc.FirstFreeSlot=DateTime.Now;
            var k= a.AddDoctor(doc);

            Assert.IsTrue(k);

            var c = a.SearchDoctorsList(null, "Janowski");


            if (c == null||c.Length==0)
                Assert.Fail();

            var dd = a.DeleteDoctor(c.First());
            Assert.IsTrue(dd);




        }
        [TestMethod]
        public void CheckAddandDeletePatient()
        {


            var a = TestingExtension.GetService();

            var doc = TestingExtension.GetPatient();

            a.AddPatient(doc);
            var c = a.GetUser(doc.User.PESEL, doc.User.Password);
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
            var c = a.GetUser(pac.User.PESEL, pac.User.Password);
            var pac1 = a.GetPatientByUserId((int)c.Key);
            var doc1 = a.SearchDoctorsList(null, "Janowski");
            if (doc1.Length == 0)
                Assert.Fail();
            var  doc2 = doc1.First();
            var date = a.GetFirstFreeSlot((int) doc2.Key);

            bool dd = a.RegisterVisit(date, (int)pac1.Key, (int)doc2.Key);
            Assert.IsTrue(dd);
            var pacviz = a.GetPatientVisits((int)pac1.Key, false);
            var docviz = a.GetDoctorVisits((int)doc2.Key, false);
            if (pacviz.Length == 0 || docviz.Length == 0)
                Assert.Fail();

            dd = a.DeleteVisit(pacviz.First());
            Assert.IsTrue(dd);

            dd = a.DeletePatient(pac1);
            Assert.IsTrue(dd);
            dd = a.DeleteDoctor(doc2);
            Assert.IsTrue(dd);


        }

        
    }
}
