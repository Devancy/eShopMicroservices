using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var config = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
	.LoadFromConfig(config)
	.AddServiceDiscoveryDestinationResolver();

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
	rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
	{
		options.Window = TimeSpan.FromSeconds(10);
		options.PermitLimit = 5;
	});
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");

app.UseRateLimiter();
app.MapReverseProxy();

app.Run();