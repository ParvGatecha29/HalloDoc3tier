using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class Payrate
    {

        public int Physicianid { get; set; }

        public int? NightShiftWeekend { get; set; } = 0;

        public int? Shift { get; set; } = 0;

        public int? HouseCallsNightWeekend { get; set; } = 0;

        public int? PhoneConsults { get; set; } = 0;

        public int? PhoneConsultsNightWeekend { get; set; } = 0;

        public int? BatchTesting { get; set; } = 0;

        public int? HouseCalls { get; set; } = 0;

    }
}
