﻿using TherapistService.DTOs;
using FluentValidation;

namespace TherapistService.Validations;

public class SpecialtyValidation : AbstractValidator<SpecialtyDto>
{
	public SpecialtyValidation()
	{
		RuleFor(s => s.Title).NotNull()
							 .NotEmpty();

		RuleFor(s => s.Content).MaximumLength(500);
	}
}
