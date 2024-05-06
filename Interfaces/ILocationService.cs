using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface ILocationService
{
	Task<Result> GetAll(Guid? parentId);

	Task<Result> Add(LocationDto model);
	Task<Result> Edit(LocationDto model);
}
