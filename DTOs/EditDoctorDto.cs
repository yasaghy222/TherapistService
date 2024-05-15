using TherapistService.Enums;

namespace TherapistService.DTOs;

public class EditTherapistDto
{
	public IFormFile? Image { get; set; }
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Family { get; set; }
	public GenderType Gender { get; set; } = GenderType.Men;
	public required int MedicalSysCode { get; set; }
	public string? Content { get; set; }
	public Guid SpecialtyId { get; set; }
}
