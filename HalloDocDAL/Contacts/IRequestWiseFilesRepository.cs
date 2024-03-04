using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
