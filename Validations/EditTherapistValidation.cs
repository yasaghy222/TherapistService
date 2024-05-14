using TherapistService.DTOs;
using FluentValidation;

namespace TherapistService.Validations;

public class EditTherapistValidation : AbstractValidator<EditTherapistDto>
{
	public EditTherapistValidation()
	{
		RuleFor(d => d.Image).SetValidator(d => new FileValidator<EditTherapistDto>(d.Image));

		RuleFor(d => d.Id).NotEmpty()
						  .NotNull();

		RuleFor(d => d.Name).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(d => d.Family).NotEmpty()
							  .NotNull()
							  .MaximumLength(100);

		RuleFor(d => d.MedicalSysCode).NotEmpty()
									  .NotNull();

		RuleFor(d => d.SpecialtyId).NotEmpty()
								   .NotNull();
	}
}
