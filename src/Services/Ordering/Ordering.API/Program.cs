using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOrderingApplication()
	.AddOrderingInfrastructure(builder.Configuration)
	.AddOrderingApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseOrderingApi();
if (app.Environment.IsDevelopment())
{
	await app.InitializeDatabaseAsync();
}

app.Run();