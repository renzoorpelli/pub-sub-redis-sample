using Feeds.Aggregator.Services;
using PubSub.Shared.Redis;
using PubSub.Shared.Redis.Streaming;
using PubSub.Shared.Serialization;
using PubSub.Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add service defaults
builder.AddServiceDefaults();

builder.Services
    .AddSerialization()
    .AddHostedService<PricingStreamBackgroundService>()
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming(); // will override the implementation of AddStreaming()

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
