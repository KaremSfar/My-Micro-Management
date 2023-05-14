namespace MicroManagement.Services.Abstraction.DTOs
{
    public record TimeSessionDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public IList<Guid> ProjectIds { get; set; }
    }
}