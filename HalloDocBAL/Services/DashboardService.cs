using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;

        public DashboardService(IRequestRepository requestRepository, IRequestWiseFilesRepository requestWiseFilesRepository)
        {
            _requestRepository = requestRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
        }

        public async Task<Requestwisefile> GetDocument(string id)
        {
            return await _requestWiseFilesRepository.GetFile(id);
        }

        public List<Dashboard> PatientDashboard(String email)
        {
            return _requestRepository.GetRequestByEmail(email);
        }

        public async Task<bool> UploadDocument(IFormFileCollection files, int rid)
        {
            foreach (var file in files)
            {
                var filePath = "";
                if (file.Length > 0)
                {
                    var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","documents", rid.ToString());
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
                        Requestid = rid,
                        Filename = file.FileName,
                        Createddate = DateTime.Now,
                        Ispatientrecords = true
                    };
                    Debug.WriteLine(file.FileName);
                    await _requestWiseFilesRepository.AddFiles(requestWiseFile);
                }
            }
            return true;
        }

        public List<Requestwisefile> ViewDocument(int id)
        {
            var obj = _requestWiseFilesRepository.GetFiles(id);
            return obj;
        }
    }
}
