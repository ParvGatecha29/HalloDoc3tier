namespace HalloDocDAL.Model
{
    public class ViewEncounterForm
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public int? IsFinalized { get; set; }
        public bool IsFinalizeDB
        {
            get
            {
                switch (IsFinalized) // Use the db status here
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public string? Location { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? DateOfService { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HistoryOfPresentIllness { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Medications { get; set; }
        public string? Allergies { get; set; }
        public string? Temperature { get; set; }
        public string? HR { get; set; }
        public string? RR { get; set; }
        public string? BPSystolic { get; set; }
        public string? BPDiastolic { get; set; }
        public string? O2 { get; set; }
        public string? Pain { get; set; }
        public string? Heent { get; set; }
        public string? CV { get; set; }
        public string? Chest { get; set; }
        public string? ABD { get; set; }
        public string? Extr { get; set; }
        public string? Skin { get; set; }
        public string? Neuro { get; set; }
        public string? Other { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? MedicationDispensed { get; set; }
        public string? Procedures { get; set; }
        public string? FollowUp { get; set; }

    }
}
