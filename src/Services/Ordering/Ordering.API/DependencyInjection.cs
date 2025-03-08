using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
	public static IServiceCollection AddOrderingApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddCarter();
		
		services.AddExceptionHandler<CustomExceptionHandler>();
		services.AddHealthChecks()
			.AddSqlServer(configuration.GetConnectionString("Database")!, name: "orderdb", tags: new string[] { "sql server" });
		
		return services;
	}

	public static WebApplication UseOrderingApi(this WebApplication app)
	{
		app.MapCarter();
		
		app.UseExceptionHandler(options => { });
		app.UseHealthChecks("/health",
			new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});
		
		return app;
	}
}