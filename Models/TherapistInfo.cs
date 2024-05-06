using TherapistService.DTOs;

namespace TherapistService.Models;

public class TherapistInfo
{
	public string Name { get; set; } = "";
	public string Family { get; set; } = "";
	public required string FullName { get; set; }

	public required int MedicalSysCode { get; set; }
	public string? Content { get; set; }

	public Guid SpecialtyId { get; set; }
	public required string SpecialtyTitle { get; set; }

	public float Rate { get; set; }
	public int RaterCount { get; set; }
	public int SuccessConsolationCount { get; set; }
	public int SuccessReservationCount { get; set; }


	public bool HasTelCounseling { get; set; } = false;
	public bool HasTextCounseling { get; set; } = false;
	public ICollection<OnlinePlanDto>? OnlinePlans { get; set; }

	public bool AcceptVisit { get; set; } = false;
	public Guid? ClinicId { get; set; }
	public ClinicDto? Clinic { get; set; }
	public ICollection<VisitPlanDto>? VisitPlans { get; set; }
	public DateTime? NearestVisitDate { get; set; }


	public TherapistLineStatus LineStatus { get; set; }
}
