using HalloDocDAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HalloDocDAL.Contacts
{
    public interface IUserRepo
    {
        Task<bool> AddUser(User user);
        Task<bool> EditUser(User user);
    }
}