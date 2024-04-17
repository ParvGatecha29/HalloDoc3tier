using HalloDocDAL.Models;
namespace HalloDocDAL.Contacts
{
    public interface IUserRepo
    {
        Task<bool> AddUser(User user);
        Task<bool> EditUser(User user);
    }
}