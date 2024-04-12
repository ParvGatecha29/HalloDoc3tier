using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocDAL.Model
{
    public class ScheduleModel
    {
            public int? Shiftid { get; set; }
            public int? ShiftDetailId { get; set; }

            public int Physicianid { get; set; }
            public string? PhysicianName { get; set; }
            public string? PhysicianPhoto { get; set; }
            public int? Regionid { get; set; }
            public string? RegionName { get; set; }

            public DateOnly Startdate { get; set; }
            public DateTime? Shiftdate { get; set; }
            public TimeOnly Starttime { get; set; }
            public TimeOnly Endtime { get; set; }

            public bool Isrepeat { get; set; }

            public string? checkWeekday { get; set; }

            public bool ShiftDeleted { get; set; }
            public int? Repeatupto { get; set; }
            public short Status { get; set; }
            public List<ScheduleModel> DayList { get; set; }
            public List<int> repeatBox { get; set; }
            public List<Region> regions { get; set; }
            public List<Physician> physicians { get; set; }
    }
}
