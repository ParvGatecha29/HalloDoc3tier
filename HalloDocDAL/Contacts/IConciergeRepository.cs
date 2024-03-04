using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Contacts
{
    public interface IConciergeRepository
    {
        Task<bool> AddConcierge(Concierge con);
    }
}
