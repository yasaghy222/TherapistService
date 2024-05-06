﻿using TherapistService.DTOs;
using FluentValidation;

namespace TherapistService.Validations;

public class OnlinePlanValidation : AbstractValidator<OnlinePlanDto>
{
	public OnlinePlanValidation()
	{
		RuleFor(op => op.StartTime).NotNull()
								   .NotEmpty();

		RuleFor(op => op.EndTime).NotNull()
								 .NotEmpty()
								 .GreaterThan(op => op.StartTime)
								 .WithMessage("ساعت پایانی نمی تواند کوچکتر از ساعت شروع باشد!");
	}
}
