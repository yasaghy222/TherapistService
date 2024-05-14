using TherapistService.DTOs;
using FluentValidation;

namespace TherapistService.Validations;

public class EditSpecialtyValidation : AbstractValidator<EditSpecialtyDto>
{
	public EditSpecialtyValidation()
	{
		RuleFor(s => s.Image).SetValidator(s => new FileValidator<EditSpecialtyDto>(s.Image));

		RuleFor(s => s.Id).NotNull()
									 .NotEmpty();

		RuleFor(s => s.Title).NotNull()
							 .NotEmpty();

		RuleFor(s => s.Content).MaximumLength(500);
	}
}
