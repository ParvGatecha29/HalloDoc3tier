using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Models;

namespace HalloDocBAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public List<Healthprofessionaltype> GetAllPrfessions()
        {
            return _ordersRepository.GetAllProfessions();
        }

        public List<Healthprofessional> GetVendorsByProfessions(int professionid)
        {
            return _ordersRepository.GetVendorsByProfession(professionid);
        }

        public Healthprofessional GetVendorById(int vendorid)
        {
            return _ordersRepository.GetVendorById(vendorid);
        }

        public bool CreateOrder(Orderdetail order)
        {
            return _ordersRepository.AddOrder(order);
        }
    }
}
