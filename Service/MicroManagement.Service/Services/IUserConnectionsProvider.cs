namespace MicroManagement.Service.WebAPI.Services
{
    public interface IUserConnectionsProvider
    {
        Task AddConnection(Guid userId, string connectionId);
        Task RemoveConnection(Guid userId, string connectionId);
        Task<bool> HasActiveConnections(Guid userId);
    }
}
