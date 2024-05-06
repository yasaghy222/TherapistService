using TherapistService.Enums;

namespace TherapistService.Entities;
public class VisitPlan : BaseEntity
{
	public DateOnly Date { get; set; }
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }

	public Guid TherapistId { get; set; }
	public required Therapist Doctor { get; set; }

	public VisitPlanStatus Status { get; set; } = VisitPlanStatus.Active;
}