using System.Collections.Concurrent;

namespace MicroManagement.Service.WebAPI.Services
{
    public class UserConnectionsProvider : IUserConnectionsProvider
    {
        private static ConcurrentDictionary<Guid, int> _usersConnections = new ConcurrentDictionary<Guid, int>();

        public Task AddConnection(Guid userId, string connectionId)
        {
            _usersConnections.AddOrUpdate(userId, 1, (key, userConnections) =>
            {
                return userConnections + 1;
            });

            return Task.CompletedTask;
        }

        public Task RemoveConnection(Guid userId, string connectionId)
        {
            if (!_usersConnections.TryGetValue(userId, out int userConnections) || userConnections == 0)
                return Task.CompletedTask;

            // Decrement the connection count for the user
            _usersConnections.TryUpdate(userId, userConnections - 1, userConnections);

            // In case all users disconnected
            if (userConnections - 1 == 0)
            {
                _usersConnections.Remove(userId, out _);
            }

            return Task.CompletedTask;
        }

        public Task<bool> HasActiveConnections(Guid userId)
        {
            if (_usersConnections.TryGetValue(userId, out int userConnections) && userConnections >= 1)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}
