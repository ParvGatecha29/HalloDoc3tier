namespace HalloDocDAL.Model
{
    public class BusinessModel
    {
        public int VendorId { get; set; }
        public string BusinessName { get; set; }
        public int? ProfessionId { get; set; }
        public string FaxNumber { get;  set; }
        public string PhoneNumber { get; set;}
        public string Email { get; set;}
        public string BusinessContact { get; set;}
        public string Street { get; set;}
        public string City { get; set;}
        public int? RegionId { get; set;}
        public string ZipCode { get; set;}
    }
}
