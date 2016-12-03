using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DatabaseAccess.Model;

namespace DataAccessService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        User GetUserByID(int value);

        [OperationContract]
        IEnumerable<Doctor> GetDoctorsList();

        [OperationContract]
        IEnumerable<Specialization> GetSpecializationsList();
        [OperationContract]
        IEnumerable<Visit> GetPatientVisits(int id);
        [OperationContract]
        IEnumerable<Visit> GetDoctorVisits(int id);
        [OperationContract]
        bool UpdatePatient(Patient toUpdate);
        [OperationContract]
        bool UpdateDoctor(Doctor toUpdate);
        [OperationContract]
        bool DeleteDoctor(Doctor toDelete);
        [OperationContract]
        bool DeletePatient(Patient toDelete);
        [OperationContract]
        bool AddPatient(Patient toAdd);
        [OperationContract]
        bool AddDoctor(Doctor toAdd);




        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "DataAccessService.ContractType".
    
}
