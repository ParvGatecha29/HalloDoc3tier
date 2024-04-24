using System;
using System.Collections.Generic;

namespace AssignmentDAL.Models;

public partial class Patient
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public int? DoctorId { get; set; }

    public int? Age { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Gender { get; set; }

    public string? Disease { get; set; }

    public string? Specialist { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
