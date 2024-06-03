namespace HalloDocDAL.Model
{
    public class AdminInvoicing
    {
        public int Id { get; set; }

        public int PhysicianId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string status { get; set; }

        public bool isApproved { get; set; }

        public bool isFinalize { get; set; }

        public string physicianName { get; set; }
    }
}
