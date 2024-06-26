﻿namespace HalloDocDAL.Models;

public partial class Request
{
    public int Requestid { get; set; }

    public int Requesttypeid { get; set; }

    public int? Userid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public short Status { get; set; }

    public int? Physicianid { get; set; }

    public string? Confirmationnumber { get; set; }

    public DateTime Createddate { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Declinedby { get; set; }

    public bool? Isurgentemailsent { get; set; }

    public DateTime? Lastwellnessdate { get; set; }

    public bool? Ismobile { get; set; }

    public short? Calltype { get; set; }

    public bool? Completedbyphysician { get; set; }

    public DateTime? Lastreservationdate { get; set; }

    public DateTime? Accepteddate { get; set; }

    public string? Relationname { get; set; }

    public string? Casenumber { get; set; }

    public string? Ip { get; set; }

    public string? Casetag { get; set; }

    public string? Casetagphysician { get; set; }

    public string? Patientaccountid { get; set; }

    public int? Createduserid { get; set; }

    public virtual ICollection<EncounterForm> EncounterForms { get; set; } = new List<EncounterForm>();

    public virtual Physician? Physician { get; set; }

    public virtual ICollection<Requestbusiness> Requestbusinesses { get; set; } = new List<Requestbusiness>();

    public virtual ICollection<Requestclient> Requestclients { get; set; } = new List<Requestclient>();

    public virtual ICollection<Requestclosed> Requestcloseds { get; set; } = new List<Requestclosed>();

    public virtual ICollection<Requestconcierge> Requestconcierges { get; set; } = new List<Requestconcierge>();

    public virtual ICollection<Requestnote> Requestnotes { get; set; } = new List<Requestnote>();

    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; set; } = new List<Requeststatuslog>();

    public virtual Requesttype Requesttype { get; set; } = null!;

    public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();
}
