using HalloDocDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Contacts
{
    public interface IRecordsRepository
    {
        public List<SearchRecord> GetSearchRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS);
        public bool DeletePatientRequest(int requestid);
        public List<PatientHistory> GetPatients(string? FirstName, string? LastName, string? Phone, string? Email);
        public List<PatientHistory> GetBlockedPatients(string? FirstName, string? LastName, string? Phone, string? Email);
        public List<PatientHistory> GetPatientRequests(int userid);
    }
}
