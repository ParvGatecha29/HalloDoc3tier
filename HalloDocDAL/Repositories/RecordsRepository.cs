using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;
using HalloDocDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Repositories
{
    public class RecordsRepository : IRecordsRepository
    {
        private readonly ApplicationDbContext _context;
        public RecordsRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public List<PatientHistory> GetPatients(string? FirstName, string? LastName, string? Phone, string? Email)
        {
            List<PatientHistory> result = (from u in _context.Users
                                               where u.Isdeleted != true
                                               select new PatientHistory
                                               {
                                                   FirstName = u.Firstname,
                                                   LastName = u.Lastname,
                                                   Email = u.Email,
                                                   PhoneNumber = u.Mobile,
                                                   Address = (u.State != null && u.Zipcode != null) ? $"{u.Street} {u.City} {u.State}-{u.Zipcode}" : "",
                                                   UserId = u.Userid
                                               }).Where(item =>
                                                   (string.IsNullOrEmpty(FirstName) || item.FirstName.ToLower().Contains(FirstName)) &&
                                                   (string.IsNullOrEmpty(LastName) || item.LastName.ToLower().Contains(LastName)) &&
                                                   (string.IsNullOrEmpty(Email)
 || item.Email.Contains(Email)) &&
                                                   (string.IsNullOrEmpty(Phone)
 || item.PhoneNumber.Contains(Phone))
                                                ).ToList();

            return result;
        }

        public List<PatientHistory> GetBlockedPatients(string? FirstName, string? LastName, string? Phone, string? Email)
        {
            List<PatientHistory> result = (from b in _context.Blockrequests
                                           join r in _context.Requests on b.Requestid equals r.Requestid
                                           select new PatientHistory
                                           {
                                               FirstName = r.Firstname,
                                               LastName = r.Lastname,
                                               Email = b.Email,
                                               PhoneNumber = b.Phonenumber,
                                               CreatedDate = b.Createddate.ToString(),
                                               IsActive = b.Isactive
                                           }).Where(item =>
                                               (string.IsNullOrEmpty(FirstName) || item.FirstName.ToLower().Contains(FirstName)) &&
                                               (string.IsNullOrEmpty(LastName) || item.LastName.ToLower().Contains(LastName)) &&
                                               (string.IsNullOrEmpty(Email)
|| item.Email.Contains(Email)) &&
                                               (string.IsNullOrEmpty(Phone)
|| item.PhoneNumber.Contains(Phone))
                                                ).ToList();

            return result;
        }
        public List<PatientHistory> GetPatientRequests(int userid)
        {
            List<PatientHistory> result = (from r in _context.Requests
                                               join p in _context.Physicians
                                               on r.Physicianid equals p.Physicianid into prJoin
                                               from p in prJoin.DefaultIfEmpty()
                                               join e in _context.EncounterForms
                                               on r.Requestid equals e.RequestId into erJoin
                                               from e in erJoin.DefaultIfEmpty()
                                               where r.Isdeleted != true && r.Userid == userid
                                               select new PatientHistory
                                               {
                                                   RequestId = r.Requestid,
                                                   ClientName = $"{r.Firstname} {r.Lastname}",
                                                   CreatedDate = r.Createddate.ToString("MMMM dd, yyyy"),
                                                   ConfirmationNumber = r.Confirmationnumber,
                                                   ProvideName = $"{p.Firstname} {p.Lastname}",
                                                   Status = r.Status,
                                                   IsFinalize = e.IsFinalize,
                                               }).ToList();
            return result;
        }
        public DateTime? GetDateofService(int requestid)
        {
            Requeststatuslog? log = _context.Requeststatuslogs.OrderByDescending(x => x.Createddate).FirstOrDefault(x => x.Requestid == requestid && x.Status == 6 && x.Physicianid != null);
            return log?.Createddate;
        }

        public DateTime? GetCloseDate(int requestid)
        {
            Requeststatuslog? log = _context.Requeststatuslogs.OrderByDescending(x => x.Createddate).FirstOrDefault(x => x.Requestid == requestid && x.Status == 9);
            return log?.Createddate;
        }
        public string? GetPatientCancellationNotes(int requestid)
        {
            Requeststatuslog? log = _context.Requeststatuslogs.OrderByDescending(x => x.Createddate).FirstOrDefault(x => x.Requestid == requestid && x.Status == 3 && x.Physicianid != null);
            return log?.Notes;
        }

        public List<SearchRecord> GetSearchRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS)
        {
            var data = (from r in _context.Requests
                        join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                        join p in _context.Physicians on r.Physicianid equals p.Physicianid into prJoin
                        from p in prJoin.DefaultIfEmpty()
                        join rn in _context.Requestnotes on r.Requestid equals rn.Requestid into rrnJoin
                        from rn in rrnJoin.DefaultIfEmpty()
                        where r.Isdeleted != true
                        select new
                        {
                            Request = r,
                            RequestClient = rc,
                            Physician = p,
                            RequestNote = rn
                        }).ToList();

            var result = data.Select(item => new SearchRecord
            {
                RequestId = item.Request.Requestid,
                PatientName = $"{item.RequestClient.Firstname} {item.RequestClient.Lastname}",
                Requestor = $"{item.Request.Firstname} {item.Request.Lastname}",
                DateOfService = GetDateofService(item.Request.Requestid),
                ServiceDate = GetDateofService(item.Request.Requestid)?.ToString("MMMM dd, yyyy") ?? "",
                DateofClose = GetCloseDate(item.Request.Requestid)?.ToString("MMMM dd, yyyy") ?? "",
                CloseDate = GetCloseDate(item.Request.Requestid),
                Email = item.RequestClient.Email,
                PhoneNumber = item.RequestClient.Phonenumber,
                Address = item.RequestClient.Location,
                Zip = item.RequestClient.Zipcode,
                RequestStatus = item.Request.Status,
                PhysicianName = item.Physician != null ? $"{item.Physician.Firstname} {item.Physician.Lastname}" : "", // Handle null Physician
                PhysicianNote = item.RequestNote?.Physiciannotes,
                CancelledByProvidor = GetPatientCancellationNotes(item.Request.Requestid),
                PatientNote = item.RequestClient.Notes,
                RequestTypeId = item.Request.Requesttypeid,
                AdminNotes = item.RequestNote?.Adminnotes
            }).ToList();


            result = result.Where(item =>
                (string.IsNullOrEmpty(Email)
 || item.Email.Contains(Email)) &&
                (string.IsNullOrEmpty(Phone)
 || item.PhoneNumber.Contains(Phone)) &&
                (string.IsNullOrEmpty(Patient) || item.PatientName.ToLower().Contains(Patient)) &&
                (string.IsNullOrEmpty(Provider) || item.PhysicianName.ToLower().Contains(Provider)) &&
                (RequestStatus == 0 || item.RequestStatus == RequestStatus) &&
                (RequestType == 0 || item.RequestTypeId == RequestType) &&
                (FromDoS == null || item.DateOfService?.Date >= FromDoS.Value.Date) &&
                (ToDoS == null || item.DateOfService?.Date <= ToDoS.Value.Date)
            ).ToList();

            return result;
        }

        public bool DeletePatientRequest(int requestid)
        {
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestid);
            if (request != null)
            {
                try
                {
                    request.Isdeleted = true;
                    request.Modifieddate = DateTime.Now;
                    _context.Requests.Update(request);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to submit Form", ex);
                }
            }
            else
            {
                return false;
            }
        }
    }
}
