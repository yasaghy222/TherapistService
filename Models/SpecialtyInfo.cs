﻿namespace TherapistService;

public class SpecialtyInfo
{
	public Guid Id { get; set; }
	public required string Title { get; set; }
	public string? Content { get; set; }
	public Guid[]? TherapistIds { get; set; }
}
