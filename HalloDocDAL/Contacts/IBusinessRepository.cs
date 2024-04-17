using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IBusinessRepository
    {
        Task<bool> AddBusiness(Business bus);
    }
}
