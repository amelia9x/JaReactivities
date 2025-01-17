namespace Domain
{
    public class ActivityAttendee
    {
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public bool IsHost { get; set; }
    }
}