namespace MicroManagement.Core
{
    public record TimeSession
    {
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public IList<Guid> ProjectIds { get; set; }
    }
}