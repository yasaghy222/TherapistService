namespace TherapistService.DTOs;

public class AddSpecialtyDto
{
	public required IFormFile Image { get; set; }
	public required string Title { get; set; }
	public string? Content { get; set; }
}
