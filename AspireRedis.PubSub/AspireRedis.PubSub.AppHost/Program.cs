using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redisContainer = builder.AddRedis("rediscache");

builder.AddProject<Feeds_Aggregator>("feeds_aggregator")
    .WithReference(redisContainer);

builder.AddProject<Feeds_Quotes>("feeds_quotes")
    .WithReference(redisContainer);

builder.Build().Run();