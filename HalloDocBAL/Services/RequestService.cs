using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDocBAL.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IConciergeRepository _conciergeRepository;
        private readonly IRequestConciergeRepository _requestConciergeRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IRequestBusinessRepository _requestBusinessRepository;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;
        private readonly ApplicationDbContext _context;

        public RequestService(ApplicationDbContext context, IRequestRepository requestRepository, IRequestClientRepository requestClientRepository, IConciergeRepository conciergeRepository, IRequestConciergeRepository requestConciergeRepository, IBusinessRepository businessRepository, IRequestBusinessRepository requestBusinessRepository, IRequestWiseFilesRepository requestWiseFilesRepository)
        {
            _requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
            _conciergeRepository = conciergeRepository;
            _requestConciergeRepository = requestConciergeRepository;
            _businessRepository = businessRepository;
            _requestBusinessRepository = requestBusinessRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
            _context = context;
        }

        public async Task<bool> PatientRequest(Req model)
        {
            int count = _context.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count() + 1;
            var request = new Request
            {
                Userid = model.userid,
                Requesttypeid = model.typeid != 0 ? model.typeid : 1,
                Firstname = model.firstName,
                Lastname = model.lastName,
                Phonenumber = model.phone,
                Email = model.email,
                Createddate = DateTime.Now,
                Confirmationnumber = "MD" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Year.ToString().Substring(2, 2) + model.lastName.Remove(2).ToUpper() + model.firstName.Remove(2).ToUpper() + count.ToString("D4")
            };
            await _requestRepository.CreateRequest(request);
            var requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = model.firstName,
                Lastname = model.lastName,
                Phonenumber = model.phone,
                Email = model.email,
                Street = model.street,
                City = model.city,
                State = model.state,
                Zipcode = model.zipcode,
                Intdate = model.date,
                Intyear = model.year,
                Strmonth = model.month
            };
            await _requestClientRepository.AddRequestClient(requestclient);
            if (model.document != null)
            {
                foreach (var file in model.document)
                {
                    var filePath = "";
                    if (file.Length > 0)
                    {
                        var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents", request.Requestid.ToString());
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }
                        filePath = Path.Combine(uploadsFolderPath, file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    if (file.FileName.Length > 0)
                    {
                        var requestWiseFile = new Requestwisefile
                        {
                            Requestid = request.Requestid,
                            Filename = file.FileName,
                            Createddate = DateTime.Now,
                            Ispatientrecords = true
                        };
                        await _requestWiseFilesRepository.AddFiles(requestWiseFile);
                    }
                }
            }
            if (model.symptoms != null)
            {
                AdminDashboardData data = new AdminDashboardData
                {
                    requestId = request.Requestid,
                    adminNotes = model.symptoms
                };
                _requestRepository.AddNotes(data);
            }
            return true;
        }

        public async Task<bool> ConciergeRequest(Req model)
        {
            int count = _context.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count() + 1;
            var request = new Request
            {
                Requesttypeid = model.typeid,
                Firstname = model.cfirstName,
                Lastname = model.clastName,
                Phonenumber = model.cphone,
                Email = model.cemail,
                Createddate = DateTime.Now,
                Confirmationnumber = "MD" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Year.ToString().Substring(2, 2) + model.lastName.Remove(2).ToUpper() + model.firstName.Remove(2).ToUpper() + count.ToString("D4")
            };
            await _requestRepository.CreateRequest(request);
            var requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = model.firstName,
                Lastname = model.lastName,
                Phonenumber = model.phone,
                Email = model.email,
                Street = model.street,
                City = model.city,
                State = model.state,
                Zipcode = model.zipcode,
                Intdate = model.dob.Day,
                Intyear = model.dob.Year,
                Strmonth = model.dob.ToString("MMMM")
            };
            await _requestClientRepository.AddRequestClient(requestclient);
            var concierge = new Concierge
            {
                Conciergename = model.cfirstName,
                Address = model.street + model.city,
                Street = model.street,
                City = model.city,
                State = model.state,
                Zipcode = model.zipcode,
                Createddate = DateTime.Now
            };
            await _conciergeRepository.AddConcierge(concierge);

            var reqcon = new Requestconcierge
            {
                Conciergeid = concierge.Conciergeid,
                Requestid = request.Requestid
            };
            return await _requestConciergeRepository.AddConciergeRequest(reqcon);
        }

        public async Task<bool> BusinessRequest(Req model)
        {
            int count = _context.Requests.Where(x => x.Createddate.Date == DateTime.Now.Date).Count() + 1;
            var request = new Request
            {
                Requesttypeid = model.typeid,
                Firstname = model.cfirstName,
                Lastname = model.clastName,
                Phonenumber = model.cphone,
                Email = model.cemail,
                Createddate = DateTime.Now,
                Confirmationnumber = "MD" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Year.ToString().Substring(2, 2) + model.lastName.Remove(2).ToUpper() + model.firstName.Remove(2).ToUpper() + count.ToString("D4")
            };
            await _requestRepository.CreateRequest(request);
            var requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = model.firstName,
                Lastname = model.lastName,
                Phonenumber = model.phone,
                Email = model.email,
                Street = model.street,
                City = model.city,
                State = model.state,
                Zipcode = model.zipcode,
                Intdate = model.dob.Day,
                Intyear = model.dob.Year,
                Strmonth = model.dob.ToString("MMMM")
            };
            await _requestClientRepository.AddRequestClient(requestclient);
            var business = new Business
            {
                Name = model.cfirstName,
                Address1 = model.street + model.city,
                City = model.city,
                Zipcode = model.zipcode,
                Createddate = DateTime.Now,
                Phonenumber = model.cphone,
                Email = model.cemail
            };
            await _businessRepository.AddBusiness(business);

            var reqbus = new Requestbusiness
            {
                Requestid = request.Requestid,
                Businessid = business.Businessid
            };
            return await _requestBusinessRepository.AddBusinessRequest(reqbus);
        }

        public bool AddNotes(AdminDashboardData data)
        {
            _requestRepository.AddNotes(data);
            return true;
        }
        public bool AcceptCase(int requestid)
        {
            var dash = new AdminDashboardData
            {
                requestId = requestid,
            };
            _requestRepository.transferRequest(dash, 2);
            return true;
        }
        public bool TransferCase(int requestid, string description, int status = 0)
        {
            var dash = new AdminDashboardData
            {
                requestId = requestid,
                notes = description
            };
            if(status == 0)
            {
                _requestRepository.transferRequest(dash, 1);
            }
            else
            {
                _requestRepository.transferRequest(dash, status);
            }
            return true;
        }

        public bool UnblockRequest(int requestid)
        {
            Blockrequest blockreq = _context.Blockrequests.FirstOrDefault(x => x.Requestid == requestid);
            if (blockreq != null)
            {
                _context.Blockrequests.Remove(blockreq);
            }
            Request req = _context.Requests.FirstOrDefault(x => x.Requestid == requestid);
            if (req != null)
            {
                req.Status = 1;
                _context.Requests.Update(req);
            }
            _context.SaveChanges();
            return true;
        }

    }
}
