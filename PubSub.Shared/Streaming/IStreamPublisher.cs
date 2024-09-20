namespace PubSub.Shared.Streaming;

public interface IStreamPublisher
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="topic">key which allow to the consumer side to subscribe to it</param>
    /// <param name="data">the data that we want to publish</param>
    /// <typeparam name="T">Generic, must be a reference type</typeparam>
    /// <returns></returns>
    Task PublishAsync<T>(string topic, T data) where T : class;
}