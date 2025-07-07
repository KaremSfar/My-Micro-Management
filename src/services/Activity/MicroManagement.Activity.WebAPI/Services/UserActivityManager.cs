using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Threading.Tasks;
using MicroManagement.Activity.WebAPI.Events;
using MicroManagement.Shared.Events;

namespace MicroManagement.Activity.WebAPI.Services
{
    public class UserActivityManager : IUserActivityManager
    {
        private readonly IUserConnectionRepository _userConnectionRepository;
        // Each user gets their own channel
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Channel<UserActivityEvent>, int>> _usersChannels = new();

        public UserActivityManager(IUserConnectionRepository userConnectionRepository)
        {
            _userConnectionRepository = userConnectionRepository;
        }

        public async Task<IAsyncEnumerable<UserActivityEvent>> GetEvents(Guid userId, CancellationToken cancellationToken = default)
        {
            var userChannels = _usersChannels.GetOrAdd(userId, _ => new());
            var newChannel = Channel.CreateUnbounded<UserActivityEvent>();
            userChannels.TryAdd(newChannel, 0);

            await _userConnectionRepository.IncrementActiveConnectionsAsync(userId);

            return ReadAllAsync(userId, newChannel, cancellationToken);
        }

        private async IAsyncEnumerable<UserActivityEvent> ReadAllAsync(Guid userId, Channel<UserActivityEvent> channel, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.Register(() => channel.Writer.Complete());

                while (await channel.Reader.WaitToReadAsync())
                {
                    while (channel.Reader.TryRead(out var evt))
                    {
                        yield return evt;
                    }
                }

                yield break;
            }
            finally
            {
                // Clean up: remove the channel from the dictionary and complete it
                if (_usersChannels.TryGetValue(userId, out var userChannels))
                {
                    userChannels.TryRemove(channel, out _);

                    await _userConnectionRepository.DecrementActiveConnectionsAsync(userId);

                    if (!userChannels.Any())
                    {
                        _usersChannels.TryRemove(userId, out _);
                    }
                }
            }
        }

        public async Task RegisterEvent(UserActivityEvent e)
        {
            if (_usersChannels.TryGetValue(e.UserId, out var userChannels))
            {
                await Task.WhenAll(userChannels.Keys.Select(c => c.Writer.WriteAsync(e).AsTask()));
            }
        }
    }
}
