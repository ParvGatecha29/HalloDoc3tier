using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IRequestBusinessRepository
    {
        Task<bool> AddBusinessRequest(Requestbusiness bus);
    }
}
