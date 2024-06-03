namespace HalloDocDAL.Models;

public partial class EncounterForm
{
    public int EncounterFormId { get; set; }

    public int? RequestId { get; set; }

    public string? HistoryOfPresentIllnessOrInjury { get; set; }

    public string? MedicalHistory { get; set; }

    public string? Medications { get; set; }

    public string? Allergies { get; set; }

    public string? Temp { get; set; }

    public string? Hr { get; set; }

    public string? Rr { get; set; }

    public string? BloodPressureSystolic { get; set; }

    public string? BloodPressureDiastolic { get; set; }

    public string? O2 { get; set; }

    public string? Pain { get; set; }

    public string? Heent { get; set; }

    public string? Cv { get; set; }

    public string? Chest { get; set; }

    public string? Abd { get; set; }

    public string? Extremeties { get; set; }

    public string? Skin { get; set; }

    public string? Neuro { get; set; }

    public string? Other { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentPlan { get; set; }

    public string? MedicationsDispensed { get; set; }

    public string? Procedures { get; set; }

    public string? FollowUp { get; set; }

    public int? AdminId { get; set; }

    public int? PhysicianId { get; set; }

    public bool IsFinalize { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Physician? Physician { get; set; }

    public virtual Request? Request { get; set; }
}
