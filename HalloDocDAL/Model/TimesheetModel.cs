using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class TimesheetModel
    {
        public int Id { get; set; }

        public string Date { get; set; } = string.Empty;

        public int shift { get; set; } = 0;

        public int NightShiftWeekend { get; set; } = 0;

        public int HouseCall { get; set; } = 0;

        public int HouseCallNightWeekend { get; set; } = 0;

        public int PhoneConsults { get; set; } = 0;

        public int PhoneConsultsNightWeekend { get; set; } = 0;

        public int BatchTesting { get; set; } = 0;
    }
}
