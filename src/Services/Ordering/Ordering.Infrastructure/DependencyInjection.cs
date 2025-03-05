using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddOrderingInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		
		return services;
	}
}