namespace HalloDocDAL.Models;

public partial class Physicianpayrate
{
    public int Id { get; set; }

    public int? PhysicianId { get; set; }

    public int? Shift { get; set; }

    public int? PhoneConsult { get; set; }

    public int? HouseCall { get; set; }

    public int? BatchTesting { get; set; }

    public int? NightShiftWeekend { get; set; }

    public int? HouseCallNightWeekend { get; set; }

    public int? PhoneConsultNightWeekend { get; set; }

    public virtual Physician? Physician { get; set; }
}
