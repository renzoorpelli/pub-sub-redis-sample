namespace PubSub.Shared.Streaming;

public interface IStreamSubscriber
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="handler">Function that receives a object and then can do anything </param>
    /// <typeparam name="T">Generic, must be a reference type</typeparam>
    /// <returns></returns>
    Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class;
}