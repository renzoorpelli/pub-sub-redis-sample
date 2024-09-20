 using PubSub.Shared.Serialization;
using PubSub.Shared.Streaming;
using StackExchange.Redis;

namespace PubSub.Shared.Redis.Streaming;

internal sealed class ReadisStreamPublisher : IStreamPublisher
{
    private readonly ISerializer _serializer;
    private readonly ISubscriber _subscriber;

    public ReadisStreamPublisher(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        _serializer = serializer;
        _subscriber = connectionMultiplexer.GetSubscriber();
    }
    public Task PublishAsync<T>(string topic, T data) where T : class
    {
        string serializedValue = _serializer.Serialize(data);
        
        return _subscriber.PublishAsync(topic, serializedValue);
    }
}