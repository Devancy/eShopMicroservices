using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull, IRequest<TResponse> // using IRequest to support both command and query
	where TResponse : notnull
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		logger.LogInformation("[START] Handling request={@Request} - response={@Response}; RequestData={RequestData}", typeof(TRequest), typeof(TResponse), request);
		var timer = new Stopwatch();
		timer.Start();

		var response = await next();

		timer.Stop();
		var timeTaken = timer.Elapsed;
		if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then log the warnings
			logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
				typeof(TRequest).Name, timeTaken.Seconds);

		logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
		return response;
	}
}