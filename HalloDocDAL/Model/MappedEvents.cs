using HalloDocDAL.Models;

namespace HalloDocDAL.Model
{
    public class MappedEvents
    {
        public int? id { get; set; }
        public int resourceId { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int? ShiftDetailId { get; set; }
        public IQueryable<Region> region { get; set; }
        public int status { get; set; }
    }
}
