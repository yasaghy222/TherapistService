using TherapistService.DTOs;
using TherapistService.Models;

namespace TherapistService.Interfaces;

public interface IVisitPlanService
{
	Task<Result> GetAll(VisitPlanFilterDto model);
	Task<Result> Add(VisitPlanDto model);
	Task<Result> Edit(VisitPlanDto model);
	Task<Result> Delete(Guid Id);
}
