namespace Ordering.API;

public static class DependencyInjection
{
	public static IServiceCollection AddOrderingApi(this IServiceCollection services)
	{
		// services.AddCarter();
		return services;
	}

	public static WebApplication UseOrderingApi(this WebApplication app)
	{
		// app.MapCarter();
		return app;
	}
}