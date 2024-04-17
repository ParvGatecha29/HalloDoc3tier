using HalloDocDAL.Contacts;
using HalloDocDAL.Data;
using HalloDocDAL.Models;

namespace HalloDocDAL.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        public OrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Healthprofessionaltype> GetAllProfessions()
        {
            return _context.Healthprofessionaltypes.ToList();
        }

        public List<Healthprofessional> GetVendorsByProfession(int professionid)
        {
            return _context.Healthprofessionals.Where(x => x.Profession == professionid).ToList();
        }

        public Healthprofessional GetVendorById(int vendorid)
        {
            return _context.Healthprofessionals.FirstOrDefault(x => x.Vendorid == vendorid);
        }
        
        public bool AddOrder(Orderdetail order)
        {
            _context.Orderdetails.Add(order);
            _context.SaveChanges();
            return true;
        }
    }
}
