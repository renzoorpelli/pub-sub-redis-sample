using PubSub.Shared.Serialization;
using PubSub.Shared.Streaming;
using StackExchange.Redis;

namespace PubSub.Shared.Redis.Streaming;

internal sealed class ReadisStreamSubscriber : IStreamSubscriber
{
    private readonly ISerializer _serializer;
    private readonly ISubscriber _subscriber;

    public ReadisStreamSubscriber(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        _serializer = serializer;
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public Task SubscribeAsync<T>(string topic, T data) where T : class
    {
        string serializedValue = _serializer.Serialize(data);

        return _subscriber.PublishAsync(topic, serializedValue);
    }

    public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class
        => _subscriber.SubscribeAsync(new RedisChannel(topic, RedisChannel.PatternMode.Literal),
            (_, data) =>
            {
                try
                {
                    var payload = _serializer.Deserialize<T>(data);

                    if (payload is null)
                    {
                        return;
                    }

                    handler(payload);
                }
                catch (Exception)
                {
                    throw;
                }
            });
}