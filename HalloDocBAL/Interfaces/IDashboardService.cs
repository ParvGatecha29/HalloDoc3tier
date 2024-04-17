using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;

namespace HalloDocBAL.Interfaces
{
    public interface IDashboardService
    {
        List<Dashboard> PatientDashboard(String email);
        List<Requestwisefile> ViewDocument(int id);
        Task<bool> UploadDocument(IFormFileCollection files, int requestid);
        Task<Requestwisefile> GetDocument(string id);
    }
}
