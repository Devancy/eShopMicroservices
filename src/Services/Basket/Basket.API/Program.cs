using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

// App services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
	config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Data services
builder.Services.AddMarten(opt =>
{
	opt.Connection(builder.Configuration.GetConnectionString("Database")!);
	// Specify the ID column for ShoppingCart
	opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(option =>
{
	option.Configuration = builder.Configuration.GetConnectionString("DistributedCache");
});

// GRPC services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
	options.Address = new Uri(builder.Configuration["Grpc:DiscountAddress"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
	var handler = new HttpClientHandler
	{
		// this should be enabled in development env only
		ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
	};
	return handler;
})
.AddResilienceHandler("grpc-pipeline", builder =>
{
	builder.AddRetry(new HttpRetryStrategyOptions
	{
		MaxRetryAttempts = 3,
		Delay = TimeSpan.FromSeconds(1),
		BackoffType = DelayBackoffType.Exponential
	});

	builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
	{
		SamplingDuration = TimeSpan.FromSeconds(30),
		FailureRatio = 0.5, // Break after 50% of requests fail
		MinimumThroughput = 10, // Minimum number of requests to evaluate
		BreakDuration = TimeSpan.FromSeconds(15) // Duration to keep the circuit open
	});

	builder.AddTimeout(TimeSpan.FromSeconds(5));
});

// Async communication services
builder.Services.AddMessageBroker(builder.Configuration);

// Cross-cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
	.AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
	.AddRedis(builder.Configuration.GetConnectionString("DistributedCache")!, name: "distributedCache", tags: ["basket", "valkey"]);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Hello World!");
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();