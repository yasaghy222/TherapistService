using TherapistService.Enums;

namespace TherapistService.Entities
{
    public class OnlinePlan : BaseEntity
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DayOfWeeks DayOfWeek { get; set; }

        public Guid TherapistId { get; set; }
        public required Therapist Therapist { get; set; }
    }
}