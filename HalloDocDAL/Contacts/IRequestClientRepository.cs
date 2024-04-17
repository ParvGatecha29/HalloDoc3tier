using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IRequestClientRepository
    {
        Task<bool> AddRequestClient(Requestclient model);
    }
}
