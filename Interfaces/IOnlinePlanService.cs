using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface IOnlinePlanService
{
	Task<Result> GetAll(Guid TherapistId);
	Task<Result> Add(OnlinePlanDto model);
	Task<Result> Edit(OnlinePlanDto model);
	Task<Result> Delete(Guid Id);
}
