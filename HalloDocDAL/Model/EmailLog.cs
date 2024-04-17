namespace HalloDocDAL.Model
{
    public class EmailLog
    {
        public int Id {  get; set; }
        public string? RecipientName { get; set; }
        public string? Action { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public DateTime cdate { get; set; }
        public DateTime sdate { get; set; }
        public string? CreatedDate { get; set; }
        public string? SentDate { get; set;}
        public string? isSent { get; set; }
        public int? sentTries { get; set; }
        public string? ConfirmationNumber { get; set; }
    }
}
