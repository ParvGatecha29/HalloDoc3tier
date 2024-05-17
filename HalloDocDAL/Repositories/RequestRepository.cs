
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDocDAL.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRequest(Request model)
        {
            Debug.WriteLine(model.Email);
            _context.Requests.Add(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public List<AdminDashboardData> GetAllRequests()
        {
            var data = _context.Requests.Join(_context.Requestclients,
                r => r.Requestid, rc => rc.Requestid,
                (r, rc) => new AdminDashboardData
                {
                    name = r.Firstname + r.Lastname,
                    dobyear = rc.Intyear,
                    dobmonth = rc.Strmonth,
                    dobdate = rc.Intdate,
                    requestor = rc.Firstname + rc.Lastname,
                    reqdate = r.Createddate,
                    phone = r.Phonenumber,
                    address = rc.Street + rc.City + rc.State,
                    requesttype = r.Requesttypeid,
                    status = r.Status,
                    email = r.Email,
                    requestId = r.Requestid,
                    requestDate = r.Createddate,
                    regionId = rc.Regionid,
                    physicianId = r.Physicianid ?? 0,
                    isDeleted = r.Isdeleted
                })
                .Where(x => x.isDeleted != true)
                .ToList();
            return data;
        }

        public async Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber, int region, string search, bool all,int physicianid)
        {
            int pageSize = 10;
           
            IQueryable<Requestclient> reqclnt;
            List<Requestclient> req;
            reqclnt = _context.Requestclients
                .Include(_ => _.Request)
                .ThenInclude(_ => _.Physician)
                .Include(_ => _.Request)
                .ThenInclude(_ => _.Requeststatuslogs)
                .Include(_ => _.Request)
                .ThenInclude(_ => _.EncounterForms)
                .Include(_ => _.Region)
                .Where(_ => status.Contains(_.Request.Status) && _.Request.Isdeleted != true)
                .OrderByDescending(x => x.Request.Createddate);

            if (reqtype != 0)
            {
                reqclnt = reqclnt.Where(_ => _.Request.Requesttypeid == reqtype);
            }
            if (region != 0)
            {
                reqclnt = reqclnt.Where(_ => _.Regionid == region).AsQueryable();
            }
            if (search.Length > 0)
            {
                reqclnt = reqclnt.Where(_ => _.Firstname.ToLower().Contains(search) || _.Lastname.ToLower().Contains(search) || _.Request.Firstname.ToLower().Contains(search)).AsQueryable();
            }
            if(physicianid != 0)
            {
                reqclnt = reqclnt.Where(_ => _.Request.Physicianid == physicianid).AsQueryable();
            }
            if (all)
            {
                req = reqclnt.ToList();
            }
            else
            {
                req = reqclnt.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            List<AdminDashboardData> abc = new List<AdminDashboardData>();

            foreach (var item in req)
            {
                var regionname = item.Region != null ? item.Region.Name : "";
                AdminDashboardData def = new AdminDashboardData
                {
                    patientId = item.Request.Userid ?? 0,
                    requestId = item.Requestid,
                    firstName = item.Firstname,
                    lastName = item.Lastname ?? "",
                    email = item.Email ?? "",
                    requestor = item.Request.Firstname ?? "",
                    status = item.Request.Status,
                    pname = item.Request.Physician != null ? item.Request.Physician.Firstname : "",
                    requesttype = item.Request.Requesttypeid,
                    dobdate = item.Intdate,
                    dobmonth = item.Strmonth ?? "",
                    dobyear = item.Intyear,
                    requestDate = item.Request.Createddate,
                    address = item.Street + ", " + item.City + ", " + regionname,
                    cphone = item.Request.Phonenumber ?? "",
                    phone = item.Phonenumber ?? "",
                    regionId = item.Regionid,
                    region = item.Region != null ? item.Region.Name : "",
                    acceptDate = item.Request.Accepteddate,
                    physicianId = item.Request.Physicianid ?? 0,
                    isFinalized = item.Request.EncounterForms.FirstOrDefault(x => x.RequestId == item.Requestid) != null ? item.Request.EncounterForms.FirstOrDefault(x => x.RequestId == item.Requestid).IsFinalize : false
                };
                foreach (var item1 in item.Request.Requeststatuslogs)
                {
                    def.notes += item1.Notes;
                }
                abc.Add(def);
            }
            
            PagedList<AdminDashboardData> result;
            if (all)
            {
                result = await PagedList<AdminDashboardData>.CreateAsync(abc, reqclnt.Count(), 1, reqclnt.Count());
            }
            else
            {
                result = await PagedList<AdminDashboardData>.CreateAsync(abc, reqclnt.Count(), pageNumber, 10);
            }
            return result;
        }

        public AdminDashboardData GetRequestById(int id)
        {
            var data = _context.Requests.Join(_context.Requestclients,
                r => r.Requestid, rc => rc.Requestid,
                (r, rc) => new AdminDashboardData
                {
                    name = r.Firstname + r.Lastname,
                    firstName = rc.Firstname,
                    lastName = rc.Lastname,
                    dobyear = rc.Intyear,
                    dobmonth = rc.Strmonth,
                    dobdate = rc.Intdate,
                    requestor = r.Firstname + r.Lastname,
                    reqdate = r.Createddate,
                    phone = r.Phonenumber,
                    address = rc.Street +", "+ rc.City +", "+ rc.State,
                    requesttype = r.Requesttypeid,
                    status = r.Status,
                    email = r.Email,
                    requestId = r.Requestid,
                    requestDate = r.Createddate,
                    regionId = rc.Regionid,
                    confirmationNo = r.Confirmationnumber,
                    isFinalized = _context.EncounterForms.FirstOrDefault(x => x.RequestId == id) != null ? _context.EncounterForms.FirstOrDefault(x => x.RequestId == id).IsFinalize  :  false,

                }).FirstOrDefault(req => req.requestId == id);
            return data;
        }

        public List<Dashboard> GetRequestByEmail(string email)
        {
            var requestData = _context.Requestclients
                .Include(_ => _.Request)
        .Where(req => req.Email == email || req.Request.Email == email)
        .Select(req => new
        {
            Request = req.Request,
            FilesGroup = _context.Requestwisefiles
                .Where(file => file.Requestid == req.Requestid)
                .ToList()
        })
        .ToList()
        .SelectMany(x => x.FilesGroup.DefaultIfEmpty(), (req, file) => new Dashboard
        {
            name = req.Request.Firstname,
            Requestid = req.Request.Requestid,
            Email = req.Request.Email,
            Createddate = req.Request.Createddate,
            Status = req.Request.Status,
            confirmationNo = req.Request.Confirmationnumber,
            Filename = file != null ? file.Filename : null,
            DocumentCount = req.FilesGroup.Count
        })
        .GroupBy(request => request.Requestid).Select(g => g.First())
        .ToList();

            return requestData;
        }

        public AdminDashboardData GetNotes(int id)
        {
            Requestnote rn = _context.Requestnotes.Include(_ => _.Request).ThenInclude(_ => _.Requeststatuslogs).FirstOrDefault(x => x.Requestid == id);
            if (rn == null)
            {
                return null;
            }
            var data = new AdminDashboardData
            {
                requestId = rn.Requestid,
                adminNotes = rn.Adminnotes,
                physicianNotes = rn.Physiciannotes,
            };

            foreach (var item in rn.Request.Requeststatuslogs)
            {
                data.transferNotes.Add(item.Notes);
            }
            return data;
        }

        public bool AddNotes(AdminDashboardData data)
        {
            var note = new Requestnote
            {
                Requestid = data.requestId,
                Adminnotes = data.adminNotes,
                Createddate = DateTime.Now,
                Createdby = "1"
            };
            _context.Requestnotes.Add(note);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateNotes(AdminDashboardData data)
        {
            var note = _context.Requestnotes.FirstOrDefault(x => x.Requestid == data.requestId);
            note.Adminnotes = data.adminNotes;
            note.Physiciannotes = data.physicianNotes;
            note.Modifieddate = DateTime.Now;
            _context.Requestnotes.Update(note);
            _context.SaveChanges();
            return true;
        }

        public bool transferRequest(AdminDashboardData data, int newstatus)
        {
            var request = _context.Requests.FirstOrDefault(x => x.Requestid == data.requestId);
            request.Status = (short)newstatus;
            request.Modifieddate = DateTime.Now;
            var rsl = new Requeststatuslog
            {
                Requestid = data.requestId,
                Status = (short)newstatus,
                Notes = data.notes,
                Createddate = DateTime.Now
            };
            if (newstatus == 2)
            {
                if(data.physicianId != 0)
                {
                    request.Status = 1;
                    rsl.Transtophysicianid = data.physicianId;
                    request.Physicianid = data.physicianId;
                }
                else
                {
                    request.Accepteddate = DateTime.Now;
                }
            }
            if (newstatus == 1)
            {
                request.Physicianid = null;
            }
            _context.Requests.Update(request);
            _context.Requeststatuslogs.Add(rsl);
            _context.SaveChanges();
            return true;
        }

        public bool AddBlockRequest(AdminDashboardData data)
        {
            var r = _context.Requests.FirstOrDefault(x => x.Requestid == data.requestId);
            var email = r.Email;
            var number = r.Phonenumber;
            var request = _context.Requests.Where(x => (x.Email == email || x.Phonenumber == number) && x.Status == 1).ToList();
            foreach (var item in request)
            {
                item.Status = 11;
                item.Modifieddate = DateTime.Now;
                var req = new Blockrequest
                {
                    Phonenumber = item.Phonenumber,
                    Email = item.Email,
                    Reason = data.notes,
                    Requestid = item.Requestid,
                    Createddate = DateTime.Now,
                };
                _context.Blockrequests.Add(req);
                _context.Requests.Update(item);
            }

            _context.SaveChanges();
            return true;
        }

        public List<Region> GetRegions()
        {
            return _context.Regions.ToList();
        }

        public bool EditCase(AdminDashboard model)
        {
            Request request = _context.Requests.FirstOrDefault(x => x.Confirmationnumber == model.request.confirmationNo);
            request.Firstname = model.request.confirmationNo;
            request.Lastname = model.request.lastName;

            _context.Requests.Update(request);
            _context.SaveChanges();
            return true;
        }

        public bool EditEncounterForm(EncounterForm model)
        {
            var res = _context.EncounterForms.FirstOrDefault(x => x.RequestId == model.RequestId);
            if (res == null)
            {
                _context.EncounterForms.Add(model);
            }
            else
            {
                res.HistoryOfPresentIllnessOrInjury = model.HistoryOfPresentIllnessOrInjury;
                res.MedicalHistory = model.MedicalHistory;
                res.Medications = model.Medications;
                res.Allergies = model.Allergies;
                res.Temp = model.Temp;
                res.Hr = model.Hr;
                res.Rr = model.Rr;
                res.BloodPressureSystolic = model.BloodPressureSystolic;
                res.BloodPressureDiastolic = model.BloodPressureDiastolic;
                res.O2 = model.O2;
                res.Pain = model.Pain;
                res.Heent = model.Heent;
                res.Cv = model.Cv;
                res.Chest = model.Chest;
                res.Abd = model.Abd;
                res.Extremeties = model.Extremeties;
                res.Skin = model.Skin;
                res.Neuro = model.Neuro;
                res.Other = model.Other;
                res.Diagnosis = model.Diagnosis;
                res.TreatmentPlan = model.TreatmentPlan;
                res.MedicationsDispensed = model.MedicationsDispensed;
                res.Procedures = model.Procedures;
                res.FollowUp = model.FollowUp;
                res.IsFinalize = model.IsFinalize;
                _context.EncounterForms.Update(res);
            }
            _context.SaveChanges();
            return true;
        }

        public EncounterForm GetEncounterForm(int requestId)
        {
            return _context.EncounterForms.FirstOrDefault(x => x.RequestId == requestId);
        }

        public bool EmailLog(string to, string subject, string body)
        {
            Emaillog log = new Emaillog
            {
                Emailtemplate = body,
                Subjectname = subject,
                Emailid = to,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = true
            };
            _context.Emaillogs.Add(log);
            _context.SaveChanges();
            return true;
        }

        public async Task<PagedList<EmailLog>> GetEmailLogs(int roleid, string receiverName, string Email, DateTime createdDate, DateTime sentDate, int pageNumber)
        {
            var data = _context.Emaillogs
                .Select(r => new EmailLog
                {
                    Id = (int)r.Emaillogid,
                    RecipientName = r.Requestid != null ?
                        _context.Requests.FirstOrDefault(p => p.Requestid == r.Requestid).Firstname + " " + _context.Requests.FirstOrDefault(p => p.Requestid == r.Requestid).Lastname
                        : (r.Physicianid != null ?
                        _context.Physicians.FirstOrDefault(p => p.Physicianid == r.Physicianid).Firstname + " " + _context.Physicians.FirstOrDefault(p => p.Physicianid == r.Physicianid).Lastname
                        : _context.Admins.FirstOrDefault(p => p.Adminid == r.Adminid).Firstname + " " + _context.Admins.FirstOrDefault(p => p.Adminid == r.Adminid).Lastname),
                    Action = r.Subjectname,
                    RoleName = _context.Roles.FirstOrDefault(p => p.Roleid == r.Roleid).Name,
                    Email = r.Emailid,
                    ConfirmationNumber = r.Confirmationnumber,
                    isSent = (bool)r.Isemailsent ? "Yes" : "No",
                    sentTries = (int)r.Senttries,
                    cdate = r.Createdate,
                    sdate = (DateTime)r.Sentdate,
                    SentDate = ((DateTime)r.Sentdate).ToString("MMM dd, yyyy"),
                    CreatedDate = r.Createdate.ToString("MMM dd, yyyy h:mm tt")
                }).AsQueryable();

            if (Email != null)
            {
                data = data.Where(r => r.Email.Contains(Email));
            }
            if (receiverName != null)
            {
                data = data.Where(r => r.RecipientName.Contains(receiverName));
            }
            if (createdDate != default(DateTime))
            {
                var date = createdDate.Date;
                data = data.Where(r => r.cdate != null && r.cdate.Date == date.Date);
            }
            if (sentDate != default(DateTime))
            {
                var date = sentDate.Date;
                data = data.Where(r => r.sdate != null && r.sdate.Date == date.Date);
            }

            List<EmailLog> list = data.ToList();

            list = list.Skip((pageNumber - 1) * 5).Take(5).ToList();

            PagedList<EmailLog> pageList = await PagedList<EmailLog>.CreateAsync(list, data.Count(), pageNumber, 5);
            return pageList;
        }
    }
}
