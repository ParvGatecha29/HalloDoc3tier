﻿
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using HalloDocDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IAdminDashboardService
    {
        List<AdminDashboardData> GetRequests();
        Task<PagedList<AdminDashboardData>> GetRequestsByStatus(int[] status, int reqtype = 0, int pageNumber = 1);
        AdminDashboardData GetRequestById(int id);
        AdminDashboardData GetNotes(int id);
        bool UpdateNotes(int id, string notes);
        bool CancelRequest(AdminDashboardData data);
        bool AssignRequest(AdminDashboardData data);
        bool AgreeRequest(AdminDashboardData data);
        bool BlockRequest(AdminDashboardData data);
        bool ClearRequest(AdminDashboardData data);
        bool CloseRequest(AdminDashboardData data);
        List<Physician> GetPhysiciansByRegion(int regionid);
        List<Region> GetAllRegions();
        bool DeleteFile(int fileid);
        bool UpdateEncounterForm(ViewEncounterForm model);
        EncounterForm GetEncounterForm(int requestId);

    }
}
