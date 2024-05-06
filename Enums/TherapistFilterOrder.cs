namespace TherapistService.Enums;

public enum TherapistFilterOrder : byte
{
	Default = 0,
	Rate = 1,
	NearestReservationTime = 2,
	SuccessReservationCount = 3,
}
