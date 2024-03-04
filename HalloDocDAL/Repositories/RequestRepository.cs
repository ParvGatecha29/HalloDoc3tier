using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
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

        public List<AdminDashboardData> GetRequestsByStatus(int[] status)
        {
            var data = _context.Requests
    // Join Requests with RequestClient
    .GroupJoin(_context.Requestclients,
               request => request.Requestid,
               client => client.Requestid,
               (request, clientGroup) => new { request, clientGroup })
    .SelectMany(
        rc => rc.clientGroup.DefaultIfEmpty(),
        (rc, client) => new { rc.request, client })
    // Join the above with RequestStatusLog
    .GroupJoin(_context.Requeststatuslogs,
               rc => rc.request.Requestid,
               status => status.Requestid,
               (rc, statusGroup) => new { rc.request, rc.client, statusGroup })
    .SelectMany(
        rcs => rcs.statusGroup.DefaultIfEmpty(),
        (rcs, status) => new { rcs.request, rcs.client, status })
    // Join the above with Physician
    .GroupJoin(_context.Physicians,
               rcs => rcs.request.Physicianid,
               physician => physician.Physicianid,
               (rcs, physicianGroup) => new { rcs.request, rcs.client, rcs.status, physicianGroup })
    .SelectMany(
        rcsp => rcsp.physicianGroup.DefaultIfEmpty(),
        (rcsp, physician) => new AdminDashboardData
        {
            requestId = rcsp.request.Requestid,
            firstName = rcsp.client != null ? rcsp.client.Firstname : null,
            lastName = rcsp.client != null ? rcsp.client.Lastname : null,
            status = rcsp.request.Status,
            requestor = rcsp.request.Firstname,
            pname = physician != null ? physician.Firstname : null,
            notes = rcsp.status != null ? rcsp.status.Notes : null,
            requesttype = rcsp.request.Requesttypeid,
            dobdate = rcsp.client != null ? rcsp.client.Intdate : null,
            dobmonth = rcsp.client != null ? rcsp.client.Strmonth : null,
            dobyear = rcsp.client != null ? rcsp.client.Intyear : null,
            email = rcsp.client != null ? rcsp.client.Email : null,
            requestDate = rcsp.request.Createddate,
            address = rcsp.client != null ? rcsp.client.Street +","+ rcsp.client.City +","+ rcsp.client.State: null,
            cphone = rcsp.request.Phonenumber,
            phone = rcsp.client !=null ? rcsp.client.Phonenumber : null,
            regionId = rcsp.client != null ? rcsp.client.Regionid : null,

        }).Where(req => status.Contains(req.status)).ToList();
            return data;
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
            var data = _context.Requeststatuslogs.Join(_context.Requestnotes,
                rsl => rsl.Requestid, rn => rn.Requestid,
                (rsl, rn) => new AdminDashboardData
                {
                    requestId = rsl.Requestid,
                    adminNotes = rn.Adminnotes,
                    physicianNotes = rn.Physiciannotes,
                    transferNotes = _context.Requeststatuslogs.Where(r=> r.Requestid == id).Select(r => r.Notes).ToList(),
                }).FirstOrDefault(x=> x.requestId == id);
            return data;
        }
        public bool AddNotes(AdminDashboardData data)
        {
            var note = new Requestnote
            {
                Requestid = data.requestId,
                Adminnotes = data.notes,
                Createddate = DateTime.Now,
                Createdby = "1"
            };
            _context.Requestnotes.Add(note);

            return true;
        }
        public bool UpdateNotes(AdminDashboardData data)
        {
            var note = _context.Requestnotes.FirstOrDefault(x=> x.Requestid == data.requestId);
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
    }
}
