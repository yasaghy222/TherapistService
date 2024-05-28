using TherapistService.DTOs;
using TherapistService.Enums;

namespace TherapistService;

public class TherapistDetail
{
	public required string ImagePath { get; set; }
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Family { get; set; }
	public required string FullName { get; set; }

	public string? Phone { get; set; }
	public bool PhoneValidate { get; set; } = false;

	public string? Email { get; set; }
	public bool EmailValidate { get; set; } = false;

	public GenderType Gender { get; set; } = GenderType.Men;

	public required int MedicalSysCode { get; set; }
	public string? Content { get; set; }

	public Guid SpecialtyId { get; set; }
	public required string SpecialtyTitle { get; set; }

	public float Rate { get; set; } = 0;
	public int RaterCount { get; set; } = 0;
	public int SuccessConsolationCount { get; set; } = 0;
	public int SuccessReservationCount { get; set; } = 0;

	public bool HasTelCounseling { get; set; } = false;
	public bool HasTextCounseling { get; set; } = false;
	public ICollection<OnlinePlanDto>? OnlinePlans { get; set; }

	public bool AcceptVisit { get; set; } = false;
	public Guid? ClinicId { get; set; }
	public ClinicDto? Clinic { get; set; }
	public ICollection<VisitPlanDto>? VisitPlans { get; set; }

	public TherapistLineStatus LineStatus { get; set; } = TherapistLineStatus.Offline;
	public TherapistStatus Status { get; set; } = TherapistStatus.NotConfirmed;
}
