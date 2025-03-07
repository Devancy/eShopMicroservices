using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddOrderingInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
		services.AddDbContext<ApplicationDbContext>((sp, options) =>
		{
			options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
			options.UseSqlServer(configuration.GetConnectionString("Database"));
		});
		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
		
		return services;
	}
}