namespace TherapistService.Enums;

public class TherapistFilterDto
{
	public TherapistFilterDto() { }

	public TherapistFilterDto(int pageIndex,
										  int pageSize,
										  TherapistServiceType serviceType,
										  TherapistFilterOrder order,
										  Guid? locationId)
	{
		PageIndex = pageIndex < 0 ? 1 : pageIndex;
		PageSize = pageSize < 0 ? 10 : pageSize;
		ServiceType = serviceType;
		Order = order;
		LocationId = locationId;
	}

	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public TherapistServiceType ServiceType { get; set; } = TherapistServiceType.All;
	public TherapistFilterOrder Order { get; set; } = TherapistFilterOrder.Default;
	public Guid? LocationId { get; set; }
}
