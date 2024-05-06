namespace TherapistService.Entities;

public class Clinic : BaseEntity
{
	public required Guid LocationId { get; set; }
	public required Location Location { get; set; }
	public required string Address { get; set; }
	public string? Content { get; set; }
	public required ICollection<Therapist> Therapists { get; set; }
}
