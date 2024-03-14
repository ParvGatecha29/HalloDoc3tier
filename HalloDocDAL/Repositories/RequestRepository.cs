
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                }).ToList();
            return data;
        }


        public async Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber)
        {
            int pageSize = 10;
            //        var data = _context.Requests
            //// Join Requests with RequestClient
            //.GroupJoin(_context.Requestclients,
            //           request => request.Requestid,
            //           client => client.Requestid,
            //           (request, clientGroup) => new { request, clientGroup })
            //.SelectMany(
            //    rc => rc.clientGroup.DefaultIfEmpty(),
            //    (rc, client) => new { rc.request, client })
            //// Join the above with RequestStatusLog
            //.GroupJoin(_context.Requeststatuslogs,
            //           rc => rc.request.Requestid,
            //           status => status.Requestid,
            //           (rc, statusGroup) => new { rc.request, rc.client, statusGroup })
            //.SelectMany(
            //    rcs => rcs.statusGroup.DefaultIfEmpty(),
            //    (rcs, status) => new { rcs.request, rcs.client, status })
            //// Join the above with Physician
            //.GroupJoin(_context.Physicians,
            //           rcs => rcs.request.Physicianid,
            //           physician => physician.Physicianid,
            //           (rcs, physicianGroup) => new { rcs.request, rcs.client, rcs.status, physicianGroup })
            //.SelectMany(
            //    rcsp => rcsp.physicianGroup.DefaultIfEmpty(),
            //    (rcsp, physician) => new AdminDashboardData
            //    {
            //        requestId = rcsp.request.Requestid,
            //        firstName = rcsp.client != null ? rcsp.client.Firstname : null,
            //        lastName = rcsp.client != null ? rcsp.client.Lastname : null,
            //        status = rcsp.request.Status,
            //        requestor = rcsp.request.Firstname,
            //        pname = physician != null ? physician.Firstname : null,
            //        notes = rcsp.status != null ? rcsp.status.Notes : null,
            //        requesttype = rcsp.request.Requesttypeid,
            //        dobdate = rcsp.client != null ? rcsp.client.Intdate : null,
            //        dobmonth = rcsp.client != null ? rcsp.client.Strmonth : null,
            //        dobyear = rcsp.client != null ? rcsp.client.Intyear : null,
            //        email = rcsp.client != null ? rcsp.client.Email : null,
            //        requestDate = rcsp.request.Createddate,
            //        address = rcsp.client != null ? rcsp.client.Street +","+ rcsp.client.City +","+ rcsp.client.State: null,
            //        cphone = rcsp.request.Phonenumber,
            //        phone = rcsp.client !=null ? rcsp.client.Phonenumber : null,
            //        regionId = rcsp.client != null ? rcsp.client.Regionid : null,

            //    }).Where(req => status.Contains(req.status)).ToList();
            IQueryable<Requestclient> reqclnt;
            List<Requestclient> req;
            if (reqtype != 0)
            {
                reqclnt = _context.Requestclients.Include(_ => _.Request).ThenInclude(_ => _.Physician).Include(_ => _.Request).ThenInclude(_ => _.Requeststatuslogs).Include(_ => _.Request).ThenInclude(_ => _.EncounterForms).Where(_ => status.Contains(_.Request.Status) && _.Request.Requesttypeid == reqtype).AsQueryable();
                req = reqclnt.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                reqclnt = _context.Requestclients.Include(_ => _.Request).ThenInclude(_ => _.Physician).Include(_ => _.Request).ThenInclude(_ => _.Requeststatuslogs).Include(_ => _.Request).ThenInclude(_ => _.EncounterForms).Where(_ => status.Contains(_.Request.Status)).AsQueryable();
                req = reqclnt.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            List<AdminDashboardData> abc = new List<AdminDashboardData>();
                         
            foreach (var item in req)
            {
                AdminDashboardData def = new AdminDashboardData
                {
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
                    address = item.Street + item.City + item.State,
                    cphone = item.Request.Phonenumber ?? "",
                    phone = item.Phonenumber ?? "",
                    regionId = item.Regionid,
                    isFinalized = item.Request.EncounterForms.FirstOrDefault(x => x.RequestId == item.Requestid) != null ? item.Request.EncounterForms.FirstOrDefault(x => x.RequestId == item.Requestid).IsFinalize : false
                };
                foreach (var item1 in item.Request.Requeststatuslogs)
                {
                    def.notes += item1.Notes;
                }
                abc.Add(def);
            }

            PagedList<AdminDashboardData> result = await PagedList<AdminDashboardData>.CreateAsync(abc, reqclnt.Count(), pageNumber,10);
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
                    address = rc.Street + rc.City + rc.State,
                    requesttype = r.Requesttypeid,
                    status = r.Status,
                    email = r.Email,
                    requestId = r.Requestid,
                    requestDate = r.Createddate,
                    regionId = rc.Regionid,
                    confirmationNo = r.Confirmationnumber

                }).FirstOrDefault(req => req.requestId == id);
            return data;
        }



        public List<Dashboard> GetRequestByEmail(string email)
        {
            var requestData = _context.Requests
        .Where(req => req.Email == email)
        .Select(req => new
        {
            Request = req,
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
                rsl.Transtophysicianid = data.physicianId;
                request.Physicianid = data.physicianId;
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

    }
}
