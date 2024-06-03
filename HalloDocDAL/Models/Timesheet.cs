namespace HalloDocDAL.Models;

public partial class Timesheet
{
    public int Id { get; set; }

    public int Invoiceid { get; set; }

    public DateTime Date { get; set; }

    public int? Totalhours { get; set; }

    public bool? Weekend { get; set; }

    public int? Housecall { get; set; }

    public int? Consult { get; set; }

    public string? Item { get; set; }

    public int? Amount { get; set; }

    public string? Billname { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Invoicing Invoice { get; set; } = null!;
}
