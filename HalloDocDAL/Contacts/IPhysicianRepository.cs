using HalloDocDAL.Model;
using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Contacts
{
    public interface IPhysicianRepository
    {
        List<Physician> GetPhysicians(int region);
        Physician GetPhysicianById(int id);
        Physician GetPhysicianByEmail(string email);
        List<Physicianregion> GetPhysicianRegions(int id);
        bool EditPhysician(Provider model);
        List<Physicianlocation> GetProviders();
    }
}
