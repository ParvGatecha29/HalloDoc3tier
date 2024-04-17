using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IRequestWiseFilesRepository
    {
        Task<bool> AddFiles(Requestwisefile rwf);
        List<Requestwisefile> GetFiles(int requestid);
        Task<Requestwisefile> GetFile(string id);
        bool DeleteFile(int id);
    }
}
