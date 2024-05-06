namespace TherapistService.DTOs;

public class VisitPlanFilterDto
{
	public VisitPlanFilterDto() { }

	public VisitPlanFilterDto(Guid therapistId, DateOnly fromDate, DateOnly toDate)
	{
		TherapistId = therapistId;
		FromDate = fromDate;
		ToDate = toDate;
	}

	public Guid TherapistId { get; set; }
	public DateOnly FromDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
	public DateOnly ToDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
}
