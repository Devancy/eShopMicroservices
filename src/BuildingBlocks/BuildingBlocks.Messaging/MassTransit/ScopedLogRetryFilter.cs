using MassTransit;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.MassTransit;

public class ScopedLogRetryFilter(ILogger<MassTransitRabbitMqConfiguration> logger) : IFilter<ConsumeContext>
{
	public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
	{
		var retryAttempt = context.GetRetryAttempt(); // 0 for initial attempt, 1+ for retries
		var messageId = context.MessageId?.ToString() ?? "unknown";

		try
		{
			if (retryAttempt > 0)
			{
				logger.LogInformation("Retrying message {MessageId}, attempt {RetryAttempt}", messageId, retryAttempt);
			}

			await next.Send(context);

			if (retryAttempt > 0)
			{
				logger.LogInformation("Message {MessageId} succeeded after {RetryAttempt} retries", messageId, retryAttempt);
			}
		}
		catch (Exception ex)
		{
			if (retryAttempt >= 0) // Log on every attempt that fails
			{
				logger.LogWarning(ex, "Message {MessageId} failed, attempt {RetryAttempt}", messageId, retryAttempt + 1);
			}
			throw; // Re-throw to let retry policy continue
		}
	}

	public void Probe(ProbeContext context) { }
}