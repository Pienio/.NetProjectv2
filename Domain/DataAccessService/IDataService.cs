using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.Model;

namespace DataAccessService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDataService : IDisposable
    {
        [OperationContract]
        User GetUserById(int value);
        [OperationContract]
        User GetUser(string pes,string password);
        [OperationContract]
        Doctor GetDoctorById(int value);
        [OperationContract]
        Doctor GetDoctorByUserId(int value);
        [OperationContract]
        Patient GetPatientById(int value);
        [OperationContract]
        Patient GetPatientByUserId(int value);
        [OperationContract]
        void Fill();
        [OperationContract]
        IEnumerable<Doctor> GetDoctorsList();
        [OperationContract]
        IEnumerable<Doctor> SearchDoctorsList(Specialization spec,string name);
        [OperationContract]
        IEnumerable<Specialization> GetSpecializationsList();
        [OperationContract]
        IEnumerable<Visit> GetPatientVisits(int id,bool arc);
        [OperationContract]
        IEnumerable<Visit> GetDoctorVisits(int id,bool arc);
        [OperationContract]
        IEnumerable<ProfileRequest> GetRequests();
        [OperationContract]
        bool UpdatePatient(Patient toUpdate);
        [OperationContract]
        bool UpdateUserPassword(int id,string pass);
        [OperationContract]
        bool UpdateDoctor(Doctor toUpdate);
        [OperationContract]
        bool UpdateSpecialization(Specialization toUpdate);
        [OperationContract]
        bool DeleteDoctor(Doctor toDelete);
        [OperationContract]
        bool DeletePatient(Patient toDelete);
        [OperationContract]
        bool DeleteSpecialization(Specialization toDelete);
        [OperationContract]
        bool DeleteRequest(ProfileRequest toDelete);
        [OperationContract]
        bool AddPatient(Patient toAdd);
        [OperationContract]
        bool AddDoctor(Doctor toAdd);
        [OperationContract]
        bool AddSpecialization(Specialization toAdd);
        [OperationContract]
        bool AddRequest(ProfileRequest toAdd);
        [OperationContract]
        bool RegisterVisit(DateTime selected,int patientId,int doctorId);
        [OperationContract]
        bool DeleteVisit(Visit ToDelete);
        [OperationContract]
        DateTime GetFirstFreeSlot(int doctorId);
        [OperationContract]
        IEnumerable<Patient> GetPatients();
        [OperationContract]
        new void Dispose();
    }
}
