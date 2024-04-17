using HalloDocDAL.Models;

namespace HalloDocBAL.Interfaces;

public interface IOrderService
{
    public List<Healthprofessionaltype> GetAllPrfessions();
    public List<Healthprofessional> GetVendorsByProfessions(int professionid);
    public Healthprofessional GetVendorById(int vendoridid);
    public bool CreateOrder(Orderdetail order);
}
