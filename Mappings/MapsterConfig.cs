using System.Reflection;
using TherapistService.Entities;
using TherapistService.Models;
using Mapster;
namespace TherapistService.Mappings;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<Specialty, SpecialtyInfo>.NewConfig().Map(dto => dto.TherapistIds,
			s => s.Therapists == null ? null : s.Therapists.Select(d => d.Id).ToArray());

		TypeAdapterConfig<Therapist, TherapistDetail>.NewConfig()
		.Map(dto => dto.FullName, s => $"{s.Name} {s.Family}")
		.Map(dto => dto.SpecialtyTitle, s => s.Specialty.Title);

		TypeAdapterConfig<Therapist, TherapistInfo>.NewConfig()
		.Map(dto => dto.FullName, s => $"{s.Name} {s.Family}")
		.Map(dto => dto.SpecialtyTitle, s => s.Specialty.Title);

		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
