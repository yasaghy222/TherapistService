using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface IClinicService
{
	Task<Result> Add(ClinicDto model);
	Task<Result> Edit(ClinicDto model);
}
