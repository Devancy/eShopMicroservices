using MassTransit;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.MassTransit;
/// <summary>
/// This class is for practical filter demo only
/// </summary>
/// <param name="logger"></param>
/// <typeparam name="T"></typeparam>
public class ScopedLogRetryFilter<T>(ILogger<MassTransitRabbitMqConfiguration> logger) : IFilter<ConsumeContext<T>>
    where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var retryAttempt = context.GetRetryAttempt();
        var messageId = context.MessageId?.ToString() ?? "unknown";

        try
        {
            if (retryAttempt > 0)
            {
                logger.LogInformation("Retrying message {MessageId} of type {MessageType} with data {@Message}, attempt {RetryAttempt}", messageId, typeof(T).Name, context.Message, retryAttempt);
            }

            await next.Send(context);

            if (retryAttempt > 0)
            {
                logger.LogInformation("Message {MessageId} of type {MessageType} succeeded after {RetryAttempt} retries", messageId, typeof(T).Name, retryAttempt);
            }
        }
        catch (Exception ex)
        {
            if (retryAttempt >= 0)
            {
                logger.LogWarning(ex, "Message {MessageId} of type {MessageType} failed, attempt {RetryAttempt}", messageId, typeof(T).Name, retryAttempt + 1);
            }
            throw;
        }
    }

    public void Probe(ProbeContext context) { }
}