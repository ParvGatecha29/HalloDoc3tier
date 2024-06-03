namespace HalloDocDAL.Model
{
    public class Timesheet
    {
        public DateTime Date { get; set; }
        public string shift { get; set; }
        public string NightShiftWeekend { get; set; }
        public string HouseCall { get; set; }
        public string HouseCallNightWeekend { get; set; }
        public int PhoneConsults { get; set; }
        public int PhoneConsultsNightWeekend { get; set; }
        public string BatchTesting { get; set; }


    }
}
