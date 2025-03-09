using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddOrderingApplication(builder.Configuration)
	.AddOrderingInfrastructure(builder.Configuration)
	.AddOrderingApi(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseOrderingApi();
if (app.Environment.IsDevelopment())
{
	await app.InitializeDatabaseAsync();
}

app.Run();