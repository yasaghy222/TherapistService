using FluentValidation;
using TherapistService.DTOs;

namespace TherapistService.Validations;

public class VisitPlanFilterValidation : AbstractValidator<VisitPlanFilterDto>
{
	public VisitPlanFilterValidation()
	{
		RuleFor(vpf => vpf.TherapistId).NotEmpty()
									.NotNull();

		RuleFor(vpf => vpf.FromDate).NotEmpty()
									.NotNull()
									.GreaterThanOrEqualTo(vpf => DateOnly.FromDateTime(DateTime.Now))
									.WithMessage("تاریخ شروع نمی تواند کمتر از تاریخ امروز باشد!");

		RuleFor(vpf => vpf.FromDate).NotEmpty()
									.NotNull()
									.GreaterThan(vpf => vpf.FromDate)
									.WithMessage("تاریخ پایان نمی تواند کمتر از تاریخ شروع باشد!");
	}
}
