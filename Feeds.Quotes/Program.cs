using System.Diagnostics;
using PubSub.Redis.Pricing.Request;
using PubSub.Redis.Pricing.Services;
using PubSub.Shared.Streaming;
using PubSub.Shared.Redis;
using PubSub.Shared.Redis.Streaming;
using PubSub.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddStreaming()
    .AddRedis(builder.Configuration)
    .AddSerialization()
    .AddRedisStreaming() // will override the implementation of AddStreaming()
    .AddSingleton<PricingRequestChannel>()
    .AddSingleton<IPricingGenerator, PricingGenerator>()
    .AddHostedService<PricingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "FeedR Quotes feed");

app.MapPost("/pricing/start", async (HttpContext context, PricingRequestChannel channel) =>
{
    var activity = Activity.Current;
    await channel.Requests.Writer.WriteAsync(new StartPricing());
    return Results.Ok();
});

app.MapPost("/pricing/stop", async (HttpContext context, PricingRequestChannel channel) =>
{
    var activity = Activity.Current;
    await channel.Requests.Writer.WriteAsync(new StopPricing());
    return Results.Ok();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
