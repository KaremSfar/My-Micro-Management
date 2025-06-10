namespace MicroManagement.Core
{
    public record TimeSession
    {
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public IList<Guid> ProjectIds { get; set; }
    }
}