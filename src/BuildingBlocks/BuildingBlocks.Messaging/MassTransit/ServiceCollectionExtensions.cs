using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.MassTransit;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers MassTransit with RabbitMQ as the message broker
	/// </summary>
	/// <param name="services">The dependency injection (DI) container in ASP.NET Core</param>
	/// <param name="configuration">The application settings provider</param>
	/// <param name="assembly">An optional parameter representing an assembly where MassTransit consumer classes are defined.</param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration,
		Assembly? assembly = null)
	{
		services.AddMassTransit(config =>
		{
			// Configures MassTransit to format queue and endpoint names in kebab-case (e.g., order-created instead of OrderCreated for OrderCreatedConsumer)
			config.SetKebabCaseEndpointNameFormatter();

			// Registers all MassTransit consumer classes (implementing IConsumer) found in the specified assembly with the DI container and MassTransit.
			if (assembly != null)
				config.AddConsumers(assembly);

			// Connects MassTransit to a RabbitMQ instance for sending and receiving messages
			// context: IBusRegistrationContext, providing access to the DI container and registration details
			// configurator: IRabbitMqBusFactoryConfigurator, used to configure RabbitMQ-specific settings
			config.UsingRabbitMq((context, configurator) =>
			{
				var logger = context.GetRequiredService<ILogger<MassTransitRabbitMqConfiguration>>();
				var host = configuration["MessageBroker:Host"] ??
				           throw new InvalidOperationException("RabbitMQ host not configured.");
				logger.LogInformation("Configuring RabbitMQ host: {Host}", host);
				configurator.Host(new Uri(host), h =>
				{
					var username = configuration["MessageBroker:UserName"] ?? "guest";
					var password = configuration["MessageBroker:Password"] ?? "guest";
					h.Username(username);
					h.Password(password);
					logger.LogDebug("RabbitMQ credentials set - Username: {Username}", username);
				});
				
				// Add retry policy for transient failures (e.g., timeouts, connection issues)
				configurator.UseMessageRetry(retry => 
				{
					var retryCount = configuration.GetValue<int>("MessageBroker:RetryCount", 5);
					var retryIntervalDelta = configuration.GetValue<int>("MessageBroker:RetryIntervalDeltaSeconds", 2);
					retry.Exponential(
						retryCount,											// 5 retries
						TimeSpan.FromSeconds(1),					// Start with 1 second
						TimeSpan.FromSeconds(30),					// Max 30 seconds
						TimeSpan.FromSeconds(retryIntervalDelta)  // Multiply delay by 2 each time
					);
					retry.Handle<Exception>(ex => ex switch
					{
						InvalidOperationException => false,
						ArgumentException => false,
						_ => true // Retry all others
					});
				});
				// Add scoped filter for retry logging
				configurator.UseConsumeFilter<ScopedLogRetryFilter>(context);
				
				// Automatically configures receive endpoints (queues) for all registered consumers based on their types and the endpoint name formatter (kebab-case here).
				// context: providing access to registered consumers and bind them to RabbitMQ queues
				// For OrderCreatedConsumer, this creates a queue named order-created and sets up message routing
				configurator.ConfigureEndpoints(context);
				logger.LogInformation("MassTransit endpoints configured with RabbitMQ for assembly: {AssemblyName}", assembly?.GetName().Name ?? "None");
			});
		});

		return services;
	}
}

// Helper class for logger category
public class MassTransitRabbitMqConfiguration { }