using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
	public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
	{
		logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

		if (await featureManager.IsEnabledAsync("OrderFulfillment"))
		{
			var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
			
			// publishing integration event for Order Fulfillment, such as billing, shipping, storage
			await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
		}
	}
}