namespace TherapistService.Entities
{
    public class Specialty : BaseEntity
    {
        public required string ImagePath { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public ICollection<Therapist>? Therapists { get; set; }
    }
}