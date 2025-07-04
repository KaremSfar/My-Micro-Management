namespace MicroManagement.Core
{
    public record TimeSession
    {
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid ProjectId { get; set; }
    }
}