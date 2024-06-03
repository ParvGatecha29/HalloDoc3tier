using HalloDocDAL.Model;
using HalloDocDAL.Repositories;

namespace HalloDocDAL.Contacts
{
    public interface IRecordsRepository
    {
        public List<SearchRecord> GetSearchRecordsAll();
        public Task<PagedList<SearchRecord>> GetSearchRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS, int pageNumber);
        public bool DeletePatientRequest(int requestid);
        public Task<PagedList<PatientHistory>> GetPatients(string? FirstName, string? LastName, string? Phone, string? Email, int pageNumber);
        public Task<PagedList<PatientHistory>> GetBlockedPatients(string? FirstName, string? LastName, string? Phone, string? Email, int pageNumber);
        public List<PatientHistory> GetPatientRequests(int userid);
    }
}
