namespace PubSub.Redis.Models;

internal sealed record CurrencyPair(string Symbol, decimal Value, long Timestamp);