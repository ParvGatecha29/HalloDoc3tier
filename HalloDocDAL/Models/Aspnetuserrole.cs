namespace HalloDocDAL.Models;

public partial class Aspnetuserrole
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual Aspnetuser User { get; set; } = null!;
}
