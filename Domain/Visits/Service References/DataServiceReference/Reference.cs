﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Visits.DataServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DataServiceReference.IDataService")]
    public interface IDataService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetUserById", ReplyAction="http://tempuri.org/IDataService/GetUserByIdResponse")]
        DatabaseAccess.Model.User GetUserById(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetUserById", ReplyAction="http://tempuri.org/IDataService/GetUserByIdResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.User> GetUserByIdAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetUser", ReplyAction="http://tempuri.org/IDataService/GetUserResponse")]
        DatabaseAccess.Model.User GetUser(string pes, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetUser", ReplyAction="http://tempuri.org/IDataService/GetUserResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.User> GetUserAsync(string pes, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorById", ReplyAction="http://tempuri.org/IDataService/GetDoctorByIdResponse")]
        DatabaseAccess.Model.Doctor GetDoctorById(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorById", ReplyAction="http://tempuri.org/IDataService/GetDoctorByIdResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor> GetDoctorByIdAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetPatientById", ReplyAction="http://tempuri.org/IDataService/GetPatientByIdResponse")]
        DatabaseAccess.Model.Patient GetPatientById(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetPatientById", ReplyAction="http://tempuri.org/IDataService/GetPatientByIdResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Patient> GetPatientByIdAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/Fill", ReplyAction="http://tempuri.org/IDataService/FillResponse")]
        void Fill();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/Fill", ReplyAction="http://tempuri.org/IDataService/FillResponse")]
        System.Threading.Tasks.Task FillAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorsList", ReplyAction="http://tempuri.org/IDataService/GetDoctorsListResponse")]
        DatabaseAccess.Model.Doctor[] GetDoctorsList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorsList", ReplyAction="http://tempuri.org/IDataService/GetDoctorsListResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor[]> GetDoctorsListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SearchDoctorsList", ReplyAction="http://tempuri.org/IDataService/SearchDoctorsListResponse")]
        DatabaseAccess.Model.Doctor[] SearchDoctorsList(DatabaseAccess.Model.Specialization spec, string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/SearchDoctorsList", ReplyAction="http://tempuri.org/IDataService/SearchDoctorsListResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor[]> SearchDoctorsListAsync(DatabaseAccess.Model.Specialization spec, string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetSpecializationsList", ReplyAction="http://tempuri.org/IDataService/GetSpecializationsListResponse")]
        DatabaseAccess.Model.Specialization[] GetSpecializationsList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetSpecializationsList", ReplyAction="http://tempuri.org/IDataService/GetSpecializationsListResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Specialization[]> GetSpecializationsListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetPatientVisits", ReplyAction="http://tempuri.org/IDataService/GetPatientVisitsResponse")]
        DatabaseAccess.Model.Visit[] GetPatientVisits(int id, bool arc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetPatientVisits", ReplyAction="http://tempuri.org/IDataService/GetPatientVisitsResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Visit[]> GetPatientVisitsAsync(int id, bool arc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorVisits", ReplyAction="http://tempuri.org/IDataService/GetDoctorVisitsResponse")]
        DatabaseAccess.Model.Visit[] GetDoctorVisits(int id, bool arc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/GetDoctorVisits", ReplyAction="http://tempuri.org/IDataService/GetDoctorVisitsResponse")]
        System.Threading.Tasks.Task<DatabaseAccess.Model.Visit[]> GetDoctorVisitsAsync(int id, bool arc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/UpdatePatient", ReplyAction="http://tempuri.org/IDataService/UpdatePatientResponse")]
        bool UpdatePatient(DatabaseAccess.Model.Patient toUpdate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/UpdatePatient", ReplyAction="http://tempuri.org/IDataService/UpdatePatientResponse")]
        System.Threading.Tasks.Task<bool> UpdatePatientAsync(DatabaseAccess.Model.Patient toUpdate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/UpdateDoctor", ReplyAction="http://tempuri.org/IDataService/UpdateDoctorResponse")]
        bool UpdateDoctor(DatabaseAccess.Model.Doctor toUpdate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/UpdateDoctor", ReplyAction="http://tempuri.org/IDataService/UpdateDoctorResponse")]
        System.Threading.Tasks.Task<bool> UpdateDoctorAsync(DatabaseAccess.Model.Doctor toUpdate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/DeleteDoctor", ReplyAction="http://tempuri.org/IDataService/DeleteDoctorResponse")]
        bool DeleteDoctor(DatabaseAccess.Model.Doctor toDelete);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/DeleteDoctor", ReplyAction="http://tempuri.org/IDataService/DeleteDoctorResponse")]
        System.Threading.Tasks.Task<bool> DeleteDoctorAsync(DatabaseAccess.Model.Doctor toDelete);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/DeletePatient", ReplyAction="http://tempuri.org/IDataService/DeletePatientResponse")]
        bool DeletePatient(DatabaseAccess.Model.Patient toDelete);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/DeletePatient", ReplyAction="http://tempuri.org/IDataService/DeletePatientResponse")]
        System.Threading.Tasks.Task<bool> DeletePatientAsync(DatabaseAccess.Model.Patient toDelete);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddPatient", ReplyAction="http://tempuri.org/IDataService/AddPatientResponse")]
        bool AddPatient(DatabaseAccess.Model.Patient toAdd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddPatient", ReplyAction="http://tempuri.org/IDataService/AddPatientResponse")]
        System.Threading.Tasks.Task<bool> AddPatientAsync(DatabaseAccess.Model.Patient toAdd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddDoctor", ReplyAction="http://tempuri.org/IDataService/AddDoctorResponse")]
        bool AddDoctor(DatabaseAccess.Model.Doctor toAdd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/AddDoctor", ReplyAction="http://tempuri.org/IDataService/AddDoctorResponse")]
        System.Threading.Tasks.Task<bool> AddDoctorAsync(DatabaseAccess.Model.Doctor toAdd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/RegisterVisit", ReplyAction="http://tempuri.org/IDataService/RegisterVisitResponse")]
        bool RegisterVisit(System.DateTime selected, int patientId, int doctorId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDataService/RegisterVisit", ReplyAction="http://tempuri.org/IDataService/RegisterVisitResponse")]
        System.Threading.Tasks.Task<bool> RegisterVisitAsync(System.DateTime selected, int patientId, int doctorId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDataServiceChannel : Visits.DataServiceReference.IDataService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataServiceClient : System.ServiceModel.ClientBase<Visits.DataServiceReference.IDataService>, Visits.DataServiceReference.IDataService {
        
        public DataServiceClient() {
        }
        
        public DataServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DatabaseAccess.Model.User GetUserById(int value) {
            return base.Channel.GetUserById(value);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.User> GetUserByIdAsync(int value) {
            return base.Channel.GetUserByIdAsync(value);
        }
        
        public DatabaseAccess.Model.User GetUser(string pes, string password) {
            return base.Channel.GetUser(pes, password);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.User> GetUserAsync(string pes, string password) {
            return base.Channel.GetUserAsync(pes, password);
        }
        
        public DatabaseAccess.Model.Doctor GetDoctorById(int value) {
            return base.Channel.GetDoctorById(value);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor> GetDoctorByIdAsync(int value) {
            return base.Channel.GetDoctorByIdAsync(value);
        }
        
        public DatabaseAccess.Model.Patient GetPatientById(int value) {
            return base.Channel.GetPatientById(value);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Patient> GetPatientByIdAsync(int value) {
            return base.Channel.GetPatientByIdAsync(value);
        }
        
        public void Fill() {
            base.Channel.Fill();
        }
        
        public System.Threading.Tasks.Task FillAsync() {
            return base.Channel.FillAsync();
        }
        
        public DatabaseAccess.Model.Doctor[] GetDoctorsList() {
            return base.Channel.GetDoctorsList();
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor[]> GetDoctorsListAsync() {
            return base.Channel.GetDoctorsListAsync();
        }
        
        public DatabaseAccess.Model.Doctor[] SearchDoctorsList(DatabaseAccess.Model.Specialization spec, string name) {
            return base.Channel.SearchDoctorsList(spec, name);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Doctor[]> SearchDoctorsListAsync(DatabaseAccess.Model.Specialization spec, string name) {
            return base.Channel.SearchDoctorsListAsync(spec, name);
        }
        
        public DatabaseAccess.Model.Specialization[] GetSpecializationsList() {
            return base.Channel.GetSpecializationsList();
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Specialization[]> GetSpecializationsListAsync() {
            return base.Channel.GetSpecializationsListAsync();
        }
        
        public DatabaseAccess.Model.Visit[] GetPatientVisits(int id, bool arc) {
            return base.Channel.GetPatientVisits(id, arc);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Visit[]> GetPatientVisitsAsync(int id, bool arc) {
            return base.Channel.GetPatientVisitsAsync(id, arc);
        }
        
        public DatabaseAccess.Model.Visit[] GetDoctorVisits(int id, bool arc) {
            return base.Channel.GetDoctorVisits(id, arc);
        }
        
        public System.Threading.Tasks.Task<DatabaseAccess.Model.Visit[]> GetDoctorVisitsAsync(int id, bool arc) {
            return base.Channel.GetDoctorVisitsAsync(id, arc);
        }
        
        public bool UpdatePatient(DatabaseAccess.Model.Patient toUpdate) {
            return base.Channel.UpdatePatient(toUpdate);
        }
        
        public System.Threading.Tasks.Task<bool> UpdatePatientAsync(DatabaseAccess.Model.Patient toUpdate) {
            return base.Channel.UpdatePatientAsync(toUpdate);
        }
        
        public bool UpdateDoctor(DatabaseAccess.Model.Doctor toUpdate) {
            return base.Channel.UpdateDoctor(toUpdate);
        }
        
        public System.Threading.Tasks.Task<bool> UpdateDoctorAsync(DatabaseAccess.Model.Doctor toUpdate) {
            return base.Channel.UpdateDoctorAsync(toUpdate);
        }
        
        public bool DeleteDoctor(DatabaseAccess.Model.Doctor toDelete) {
            return base.Channel.DeleteDoctor(toDelete);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteDoctorAsync(DatabaseAccess.Model.Doctor toDelete) {
            return base.Channel.DeleteDoctorAsync(toDelete);
        }
        
        public bool DeletePatient(DatabaseAccess.Model.Patient toDelete) {
            return base.Channel.DeletePatient(toDelete);
        }
        
        public System.Threading.Tasks.Task<bool> DeletePatientAsync(DatabaseAccess.Model.Patient toDelete) {
            return base.Channel.DeletePatientAsync(toDelete);
        }
        
        public bool AddPatient(DatabaseAccess.Model.Patient toAdd) {
            return base.Channel.AddPatient(toAdd);
        }
        
        public System.Threading.Tasks.Task<bool> AddPatientAsync(DatabaseAccess.Model.Patient toAdd) {
            return base.Channel.AddPatientAsync(toAdd);
        }
        
        public bool AddDoctor(DatabaseAccess.Model.Doctor toAdd) {
            return base.Channel.AddDoctor(toAdd);
        }
        
        public System.Threading.Tasks.Task<bool> AddDoctorAsync(DatabaseAccess.Model.Doctor toAdd) {
            return base.Channel.AddDoctorAsync(toAdd);
        }
        
        public bool RegisterVisit(System.DateTime selected, int patientId, int doctorId) {
            return base.Channel.RegisterVisit(selected, patientId, doctorId);
        }
        
        public System.Threading.Tasks.Task<bool> RegisterVisitAsync(System.DateTime selected, int patientId, int doctorId) {
            return base.Channel.RegisterVisitAsync(selected, patientId, doctorId);
        }
    }
}
