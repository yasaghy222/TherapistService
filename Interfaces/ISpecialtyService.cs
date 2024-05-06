using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface ISpecialtyService
{
	Task<Result> GetAll();
	Task<Result> Add(SpecialtyDto model);
	Task<Result> Edit(SpecialtyDto model);
}
