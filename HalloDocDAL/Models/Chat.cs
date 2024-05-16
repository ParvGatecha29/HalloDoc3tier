using System;
using System.Collections.Generic;

namespace HalloDocDAL.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int Requestid { get; set; }

    public int? Adminid { get; set; }

    public int? Physicianid { get; set; }

    public int? Patientid { get; set; }

    public DateTime Createddate { get; set; }

    public int Createdby { get; set; }

    public virtual ICollection<Chatdatum> Chatdata { get; set; } = new List<Chatdatum>();
}
