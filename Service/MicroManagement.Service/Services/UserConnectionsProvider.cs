using System.Collections.Concurrent;

namespace MicroManagement.Service.WebAPI.Services
{
    public class UserConnectionsProvider : IUserConnectionsProvider
    {
        private static ConcurrentDictionary<Guid, int> _usersConnections = new ConcurrentDictionary<Guid, int>();

        public Task AddConnection(Guid userId, string connectionId)
        {
            _usersConnections.AddOrUpdate(userId, 1, (key, userConnections) => userConnections + 1);

            return Task.CompletedTask;
        }

        public Task RemoveConnection(Guid userId, string connectionId)
        {
            // Decrement the connection count for the user
            int newCount = _usersConnections.AddOrUpdate(userId, addValue: 0, (userId, numberOfConnections) => numberOfConnections - 1);

            if (newCount <= 0)
                _usersConnections.TryRemove(new(userId, newCount));


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
