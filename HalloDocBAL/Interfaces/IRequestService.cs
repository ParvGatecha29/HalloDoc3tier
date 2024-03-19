using HalloDocDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IRequestService
    {
        Task<bool> PatientRequest(Req model);
        Task<bool> ConciergeRequest(Req model);
        Task<bool> BusinessRequest(Req model);
        bool AddNotes(AdminDashboardData data);
    }
}
