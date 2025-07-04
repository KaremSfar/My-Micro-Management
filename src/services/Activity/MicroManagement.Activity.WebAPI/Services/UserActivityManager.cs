using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using MicroManagement.Activity.WebAPI.Events;
using MicroManagement.Shared.Events;

namespace MicroManagement.Activity.WebAPI.Services
{
    public class UserActivityManager
    {
        // Each user gets their own channel
        private readonly ConcurrentDictionary<Guid, Channel<UserActivityEvent>> _userChannels = new();

        public IAsyncEnumerable<UserActivityEvent> GetEvents(Guid userId, CancellationToken cancellationToken = default)
        {
            var channel = _userChannels.GetOrAdd(userId, _ => Channel.CreateUnbounded<UserActivityEvent>());
            return ReadAllAsync(userId, channel, cancellationToken);
        }

        private async IAsyncEnumerable<UserActivityEvent> ReadAllAsync(Guid userId, Channel<UserActivityEvent> channel, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.Register(() => channel.Writer.Complete());

                while (await channel.Reader.WaitToReadAsync(cancellationToken))
                {
                    while (channel.Reader.TryRead(out var evt))
                    {
                        yield return evt;
                    }
                }
            }
            finally
            {
                // Clean up: remove the channel from the dictionary and complete it
                if (_userChannels.TryRemove(userId, out var removedChannel))
                {
                    removedChannel.Writer.Complete();
                }
            }
        }

        public async Task RegisterEvent(UserActivityEvent e)
        {
            if (_userChannels.TryGetValue(e.UserId, out var channel))
            {
                await channel.Writer.WriteAsync(e);
            }
        }
    }
}
