using HalloDocDAL.Models;

namespace HalloDocDAL.Model
{
    public class CustomModel
    {
        public User user { get; set; }
        public List<Requestwisefile> requests { get; set; }
        public List<Dashboard> dashboards { get; set; }
    }
}
