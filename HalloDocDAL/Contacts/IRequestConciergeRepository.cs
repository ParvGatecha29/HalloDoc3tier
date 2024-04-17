using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IRequestConciergeRepository
    {
        Task<bool> AddConciergeRequest(Requestconcierge con);
    }
}
