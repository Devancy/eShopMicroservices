using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOrderingApplication()
	.AddOrderingInfrastructure(builder.Configuration)
	.AddOrderingApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();