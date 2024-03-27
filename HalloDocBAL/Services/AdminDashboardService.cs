
using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserRepository _userRepository;

        public AdminDashboardService(IRequestRepository requestRepository, IPhysicianRepository physicianRepository, IRequestWiseFilesRepository requestWiseFilesRepository, IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _physicianRepository = physicianRepository;
            _requestWiseFilesRepository = requestWiseFilesRepository;
            _userRepository = userRepository;
        }
        public List<AdminDashboardData> GetRequests()
        {
            var data = _requestRepository.GetAllRequests();
            return data;
        }

        public async Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype, int pageNumber, int region, string search, bool all)
        {
            var data = await _requestRepository.GetRequestsByStatus(status, reqtype, pageNumber, region, search,all);
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

        public Physician GetPhysiciansById(int id)
        {
            return _physicianRepository.GetPhysicianById(id);
        }

        public Physician GetPhysiciansByEmail(string email)
        {
            return _physicianRepository.GetPhysicianByEmail(email);
        }

        public List<Physicianregion> GetPhysicianRegions(int id)
        {
            return _physicianRepository.GetPhysicianRegions(id);
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

        public bool CloseRequest(AdminDashboardData data)
        {
            _requestRepository.transferRequest(data, 9);
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

        public bool UpdateEncounterForm(ViewEncounterForm data)
        {
            var model = new EncounterForm
            {
                RequestId = data.RequestId,
                HistoryOfPresentIllnessOrInjury = data.HistoryOfPresentIllness,
                MedicalHistory = data.MedicalHistory,
                Medications = data.Medications,
                Allergies = data.Allergies,
                Temp = data.Temperature,
                Hr = data.HR,
                Rr = data.RR,
                BloodPressureSystolic = data.BPSystolic,
                BloodPressureDiastolic = data.BPDiastolic,
                O2 = data.O2,
                Pain = data.Pain,
                Heent = data.Heent,
                Cv = data.CV,
                Chest = data.Chest,
                Abd = data.ABD,
                Extremeties = data.Extr,
                Skin = data.Skin,
                Neuro = data.Neuro,
                Other = data.Other,
                Diagnosis = data.Diagnosis,
                TreatmentPlan = data.TreatmentPlan,
                MedicationsDispensed = data.MedicationDispensed,
                Procedures = data.Procedures,
                FollowUp = data.FollowUp,
                IsFinalize = data.IsFinalizeDB
            };
            _requestRepository.EditEncounterForm(model);
            return true;
        }

        public EncounterForm GetEncounterForm(int requestId)
        {
            var data = _requestRepository.GetEncounterForm(requestId);
            return data;
        }

        public Admin GetAdminById(string id)
        {
            return _userRepository.GetAdminById(id);
        }
        public bool UpdateProfile(AdminProfile model)
        {
            _userRepository.UpdateAdminProfile(model);
            return true;
        }

        public List<int> GetAdminRegions(string id)
        {
            return _userRepository.GetAdminRegions(id);
        }

        public bool AddProvider(Provider model, string adminId)
        {
            bool add = _userRepository.AddProvider(model, adminId);
            return add;
        }

        public bool EditPhysician(Provider model)
        {
            bool add = _physicianRepository.EditPhysician(model);
            return add;
        }

        public List<Physicianlocation> GetProviders()
        {
            var add = _physicianRepository.GetProviders();
            return add;
        }
    }
}
