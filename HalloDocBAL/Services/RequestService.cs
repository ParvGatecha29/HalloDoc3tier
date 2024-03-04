using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public RequestService(ApplicationDbContext context, IRequestRepository requestRepository,IRequestClientRepository requestClientRepository, IConciergeRepository conciergeRepository, IRequestConciergeRepository requestConciergeRepository, IBusinessRepository businessRepository, IRequestBusinessRepository requestBusinessRepository, IRequestWiseFilesRepository requestWiseFilesRepository)
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
            int count = _context.Requests.Where(x=> x.Createddate.Date == DateTime.Now.Date).Count() + 1;
            var request = new Request
            {
                Userid = model.userid,
                Requesttypeid = model.typeid,
                Firstname = model.firstName,
                Lastname = model.lastName,
                Phonenumber = model.phone,
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
                Intdate = model.date,
                Intyear = model.year,
                Strmonth = model.month
            };
            await _requestClientRepository.AddRequestClient(requestclient);
            foreach(var file in model.document)
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
                Intdate = model.date,
                Intyear = model.year,
                Strmonth = model.month
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
                Intdate = model.date,
                Intyear = model.year,
                Strmonth = model.month
            };
            await _requestClientRepository.AddRequestClient(requestclient);
            var business = new Business
            {
                Name = model.cfirstName,
                Address1 = model.street + model.city,
                City = model.city,
                Zipcode = model.zipcode,
                Createddate = DateTime.Now,
                Phonenumber = model.cphone
            };
            await _businessRepository.AddBusiness(business);

            var reqbus = new Requestbusiness
            {
                Requestid = request.Requestid,
                Businessid = business.Businessid
            };
            return await _requestBusinessRepository.AddBusinessRequest(reqbus);
        }
    }
}
