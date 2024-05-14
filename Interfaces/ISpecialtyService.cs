using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface ISpecialtyService
{
	Task<Result> GetAll();
	Task<Result> Add(AddSpecialtyDto model);
	Task<Result> Edit(EditSpecialtyDto model);
}
