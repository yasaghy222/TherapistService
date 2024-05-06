using TherapistService;
using TherapistService.DTOs;
using TherapistService.Entities;
using TherapistService.Enums;
using TherapistService.Models;

namespace MedicalHealthPlus.Interfaces;

public interface ITherapistService
{
	Task<Result> GetInfo(Guid id);
	Task<Result> GetDetail(Guid id);

	Task<Result> GetAllRecommends();
	Task<Result> GetAllInfo(TherapistFilterDto model);
	Task<Result> GetAllDetails(TherapistFilterDto model);

	Task<Result> Add(AddTherapistDto model);
	Task<Result> Edit(EditTherapistDto model);
	Task<Result> ChangeStatus(Guid id, TherapistStatus status);
}
