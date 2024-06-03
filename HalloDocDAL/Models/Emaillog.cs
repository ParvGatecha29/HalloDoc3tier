namespace HalloDocDAL.Models;

public partial class Emaillog
{
    public long Emaillogid { get; set; }

    public string? Emailtemplate { get; set; }

    public string Subjectname { get; set; } = null!;

    public string Emailid { get; set; } = null!;

    public string? Confirmationnumber { get; set; }

    public string? Filepath { get; set; }

    public int? Roleid { get; set; }

    public int? Requestid { get; set; }

    public int? Adminid { get; set; }

    public int? Physicianid { get; set; }

    public DateTime Createdate { get; set; }

    public DateTime? Sentdate { get; set; }

    public bool? Isemailsent { get; set; }

    public int? Senttries { get; set; }

    public int? Action { get; set; }
}
