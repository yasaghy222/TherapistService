namespace TherapistService.Enums;

public class TherapistFilterDto
{
	public TherapistFilterDto() { }

	public TherapistFilterDto(TherapistServiceType serviceType,
											TherapistFilterOrder order,
											Guid? locationId)
	{
		ServiceType = serviceType;
		Order = order;
		LocationId = locationId;
	}

	public TherapistServiceType ServiceType { get; set; } = TherapistServiceType.All;
	public TherapistFilterOrder Order { get; set; } = TherapistFilterOrder.Default;
	public Guid? LocationId { get; set; }
}
