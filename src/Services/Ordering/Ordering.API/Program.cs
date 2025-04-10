using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services
	.AddOrderingApplication(builder.Configuration)
	.AddOrderingInfrastructure(builder.Configuration)
	.AddOrderingApi(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Hello World!");

app.UseOrderingApi();
if (app.Environment.IsDevelopment())
{
	await app.InitializeDatabaseAsync();
}

app.Run();