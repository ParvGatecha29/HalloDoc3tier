using System;
using System.Collections.Generic;

namespace HalloDocDAL.Models;

public partial class Invoicing
{
    public int Id { get; set; }

    public DateTime Startdate { get; set; }

    public DateTime Enddate { get; set; }

    public int Physicianid { get; set; }

    public int? Invoicetotal { get; set; }

    public bool Isfinalize { get; set; }

    public bool Isapproved { get; set; }

    public int Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public int Modifiedby { get; set; }

    public DateTime Modifieddate { get; set; }

    public int? Bonus { get; set; }

    public string? Admindescription { get; set; }

    public virtual Physician Physician { get; set; } = null!;

    public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
}
