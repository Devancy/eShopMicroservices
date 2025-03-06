using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddOrderingInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options => 
			options.UseSqlServer(configuration.GetConnectionString("Database")));
		// services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
		
		return services;
	}
}