public interface IUserConnectionRepository
{
    Task<int> GetActiveConnectionsAsync(Guid userId);
    Task IncrementActiveConnectionsAsync(Guid userId);
    Task DecrementActiveConnectionsAsync(Guid userId);
    Task<IEnumerable<(Guid UserId, int Count, DateTime? UpdatedAt)>> GetAllInactiveUsersAsync();
}