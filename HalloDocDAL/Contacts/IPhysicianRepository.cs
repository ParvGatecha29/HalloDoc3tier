using HalloDocDAL.Model;
using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IPhysicianRepository
    {
        List<Physician> GetPhysicians(int region);
        Physician GetPhysicianById(int id);
        Physician GetPhysicianByAspId(string id);
        Physician GetPhysicianByEmail(string email);
        List<Physicianregion> GetPhysicianRegions(int id);
        bool EditPhysician(Provider model);
        List<Physicianlocation> GetProviders();
        Physicianpayrate GetPayRate(int id);
        bool SavePayRate(int Physicianid, int rate, int type);
    }
}
