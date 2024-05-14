﻿namespace TherapistService.Models;

public class RecommendedTherapist
{
	public required string ImagePath { get; set; }
	public Guid Id { get; set; }
	public required string FullName { get; set; }
	public required string SpecialtyTitle { get; set; }
	public int SuccessConsolationCount { get; set; }
	public float Rate { get; set; }

	public TherapistLineStatus LineStatus { get; set; }
}
