﻿using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IRequestWiseFilesRepository _requestWiseFilesRepository;

        public AdminDashboardService(IRequestRepository requestRepository, IPhysicianRepository physicianRepository, IRequestWiseFilesRepository requestWiseFilesRepository)
        {
            _requestRepository = requestRepository;
            _physicianRepository = physicianRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
        }
        public List<AdminDashboardData> GetRequests()
        {
            var data = _requestRepository.GetAllRequests();
            return data;
        }

        public List<AdminDashboardData> GetRequestsByStatus(int[] status)
        {
            var data = _requestRepository.GetRequestsByStatus(status);
            return data;
        }
        public AdminDashboardData GetRequestById(int id)
        {
            var data = _requestRepository.GetRequestById(id);
            return data;
        }

        public AdminDashboardData GetNotes(int id)
        {
            var data = _requestRepository.GetNotes(id);
            if(data == null)
            {
                data = new AdminDashboardData
                {
                    requestId = id,
                    adminNotes = "",
                    physicianNotes = "",
                    transferNotes = null,
                    confirmationNo = ""
                };
                return data;
            }
            return data;
        }

        public bool UpdateNotes(int id, string notes)
        {
            var data = _requestRepository.GetNotes(id);
            if (data == null)
            {
                data = new AdminDashboardData
                {
                    requestId = id,
                    adminNotes = notes,
                    physicianNotes = "",
                    transferNotes = null
                };
                _requestRepository.AddNotes(data);
                return true;
            }
            data.adminNotes = notes;
            _requestRepository.UpdateNotes(data);
            return true;
        }

        public bool CancelRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data,3);
            return true;
        }

        public List<Physician> GetPhysiciansByRegion(int regionid)
        {
            return _physicianRepository.GetPhysicians(regionid);
        }

        public bool AssignRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 2);
            return true;
        }

        public bool AgreeRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 4);
            return true;
        }

        public bool BlockRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 11);
            _requestRepository.AddBlockRequest(data);
            return true;
        }

        public bool ClearRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 10);
            return true;
        }

        public List<Region> GetAllRegions()
        {
            return _requestRepository.GetRegions();
        }

        public bool DeleteFile(int fileid)
        {
            _requestWiseFilesRepository.DeleteFile(fileid);
            return true;
        }
    }
}
