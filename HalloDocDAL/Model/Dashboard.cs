namespace HalloDocDAL.Model
{
    public class Dashboard
    {
        public string name { get; set; }
        public string Email { get; set; }
        public string confirmationNo { get; set; }
        public DateTime Createddate { get; set; }
        public bool? Ispatientrecords { get; set; }
        public short Status { get; set; }
        public int Requestid { get; internal set; }
        public string? Filename { get; internal set; }
        public int DocumentCount { get; internal set; }
    }
}
