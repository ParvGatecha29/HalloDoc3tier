using HalloDocDAL.Models;

namespace HalloDocDAL.Contacts
{
    public interface IOrdersRepository
    {
        public List<Healthprofessionaltype> GetAllProfessions();
        public List<Healthprofessional> GetVendorsByProfession(int professionid);
        public Healthprofessional GetVendorById(int vendorid);
        public bool AddOrder(Orderdetail order);
    }
}
