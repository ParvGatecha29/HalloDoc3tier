using Microsoft.AspNetCore.Http;

namespace HalloDocDAL.Model
{
    public class InvoicingModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public string Date { get; set; } = string.Empty;

        public int OnCallHours { get; set; } = 0;

        public int TotalHours { get; set; } = 0;

        public bool isWeekEnd { get; set; } = false;

        public int HouseCall { get; set; } = 0;

        public int Consult { get; set; } = 0;

        public string Item { get; set; } = string.Empty;

        public int Amount { get; set; } = 0;

        public string BillName { get; set; } = string.Empty;

        public IFormFile? Bill { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
