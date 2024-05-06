using TherapistService.DTOs;
using FluentValidation;

namespace TherapistService.Validations;

public class LocationValidation : AbstractValidator<LocationDto>
{
	public LocationValidation()
	{
		RuleFor(l => l.Title).NotNull()
							 .NotEmpty();

		RuleFor(l => l.Latitudes).NotEmpty();

		RuleFor(l => l.Longitudes).NotEmpty();
	}
}
