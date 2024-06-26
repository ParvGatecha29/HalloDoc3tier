﻿namespace HalloDocDAL.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Aspnetuserid { get; set; }

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public bool? Ismobile { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? Regionid { get; set; }

    public string? Zipcode { get; set; }

    public string? Strmonth { get; set; }

    public int? Intyear { get; set; }

    public int? Intdate { get; set; }

    public string? Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Ip { get; set; }

    public bool? Isrequestwithemail { get; set; }

    public virtual Aspnetuser? Aspnetuser { get; set; }

    public virtual Aspnetuser? CreatedbyNavigation { get; set; }

    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    public virtual Region? Region { get; set; }
}
