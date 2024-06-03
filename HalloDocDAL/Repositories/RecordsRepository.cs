using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDocDAL.Repositories
{
    public class RecordsRepository : IRecordsRepository
    {
        private readonly ApplicationDbContext _context;

        public RecordsRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<PagedList<PatientHistory>> GetPatients(string? FirstName, string? LastName, string? Phone, string? Email, int pageNumber)
        {
            var result = (from u in _context.Users
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
                              (string.IsNullOrEmpty(Email) || item.Email.Contains(Email)) &&
                              (string.IsNullOrEmpty(Phone) || item.PhoneNumber.Contains(Phone))
                          );
            var resultl = result.Skip((pageNumber - 1) * 10).Take(10).ToList();
            PagedList<PatientHistory> resultlist;
            resultlist = await PagedList<PatientHistory>.CreateAsync(resultl, result.Count(), pageNumber, 10);

            return resultlist;
        }

        public async Task<PagedList<PatientHistory>> GetBlockedPatients(string? FirstName, string? LastName, string? Phone, string? Email, int pageNumber = 1)
        {
            var result = (from b in _context.Blockrequests
                          join r in _context.Requests on b.Requestid equals r.Requestid
                          select new PatientHistory
                          {
                              FirstName = r.Firstname,
                              LastName = r.Lastname,
                              Email = b.Email,
                              PhoneNumber = b.Phonenumber,
                              CreatedDate = b.Createddate.ToString(),
                              IsActive = b.Isactive,
                              RequestId = r.Requestid
                          }).Where(item =>
                              (string.IsNullOrEmpty(FirstName) || item.FirstName.ToLower().Contains(FirstName)) &&
                              (string.IsNullOrEmpty(LastName) || item.LastName.ToLower().Contains(LastName)) &&
                              (string.IsNullOrEmpty(Email) || item.Email.Contains(Email)) &&
                              (string.IsNullOrEmpty(Phone) || item.PhoneNumber.Contains(Phone))
                                                );

            var resultl = result.Skip((pageNumber - 1) * 10).Take(10).ToList();
            PagedList<PatientHistory> resultlist;
            resultlist = await PagedList<PatientHistory>.CreateAsync(resultl, result.Count(), pageNumber, 10);

            return resultlist;
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

        public List<SearchRecord> GetSearchRecordsAll()
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
            return result;
        }

        public async Task<PagedList<SearchRecord>> GetSearchRecords(string? Email, DateTime? FromDoS, string? Phone, string? Patient, string? Provider, int RequestStatus, int RequestType, DateTime? ToDoS, int pageNumber)
        {
            var data = _context.Requests
                .Include(r => r.Requestclients)
                .Include(r => r.Requestnotes)
                .Where(r => r.Isdeleted != true)
                .Select(r => new SearchRecord
                {
                    RequestId = r.Requestid,
                    Requestor = r.Firstname + r.Lastname,
                    PatientName = r.Requestclients.FirstOrDefault().Firstname + " " + r.Requestclients.FirstOrDefault().Lastname,
                    Email = r.Requestclients.FirstOrDefault().Email,
                    PhoneNumber = r.Requestclients.FirstOrDefault().Phonenumber,
                    Address = r.Requestclients.FirstOrDefault().Address,
                    Zip = r.Requestclients.FirstOrDefault().Zipcode,
                    RequestStatus = r.Status,
                    PhysicianName = _context.Physicians.FirstOrDefault(n => n.Physicianid == r.Physicianid).Firstname != null ? _context.Physicians.FirstOrDefault(n => n.Physicianid == r.Physicianid).Firstname : "",
                    PhysicianNote = r.Requestnotes.FirstOrDefault().Physiciannotes,
                    AdminNotes = r.Requestnotes.FirstOrDefault().Adminnotes,
                    PatientNote = r.Requestclients.FirstOrDefault().Notes,
                    RequestTypeId = r.Requesttypeid
                }).AsQueryable();

            if (RequestStatus != 0)
            {
                data = data.Where(r => r.RequestStatus == RequestStatus);
            }
            if (Patient != null)
            {
                data = data.Where(r => r.PatientName.Contains(Patient));
            }
            if (RequestType != 0)
            {
                data = data.Where(r => r.RequestTypeId == RequestType);
            }
            if (Provider != null)
            {
                data = data.Where(r => r.PhysicianName.Contains(Provider));
            }
            if (Email != null)
            {
                data = data.Where(r => r.Email == Email);
            }
            if (Phone != null)
            {
                data = data.Where(r => r.PhoneNumber == Phone);
            }

            List<SearchRecord> list = data.ToList();

            list = list.Skip((pageNumber - 1) * 10).Take(10).ToList();

            PagedList<SearchRecord> pageList = await PagedList<SearchRecord>.CreateAsync(list, data.Count(), pageNumber, 10);
            return pageList;
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
