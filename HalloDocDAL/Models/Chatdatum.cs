using System;
using System.Collections.Generic;

namespace HalloDocDAL.Models;

public partial class Chatdatum
{
    public int Id { get; set; }

    public int Chatid { get; set; }

    public string Message { get; set; } = null!;

    public string Messageby { get; set; } = null!;

    public DateTime Date { get; set; }

    public bool? Isdeleted { get; set; }

    public string Sendername { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;
}
