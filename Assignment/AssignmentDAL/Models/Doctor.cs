using System;
using System.Collections.Generic;

namespace AssignmentDAL.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string? Specialist { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
