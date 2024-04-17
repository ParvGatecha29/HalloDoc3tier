using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IConciergeRepository
    {
        Task<bool> AddConcierge(Concierge con);
    }
}
