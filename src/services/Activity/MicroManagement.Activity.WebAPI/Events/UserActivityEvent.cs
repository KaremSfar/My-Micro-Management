namespace MicroManagement.Activity.WebAPI.Events
{
    public record UserActivityEvent
    {
        public UserActivityEventType UserActivityEventType { get; set; }
        public Guid UserId { get; set; }
        public dynamic EventData { get; set; } // TODO-KAREM: Replace dynamic with a proper type or interface
    }
}
