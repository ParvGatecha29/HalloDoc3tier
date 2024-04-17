using HalloDocDAL.Models;

namespace HalloDocDAL.Model
{
    public class ViewUploads
    {
        public AdminDashboardData Request { get; set; }
        public List<Requestwisefile> Requestwisefiles { get; set; }
    }
}
