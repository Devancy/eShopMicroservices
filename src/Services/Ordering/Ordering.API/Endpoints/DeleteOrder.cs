using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

// public record DeleteOrderRequest(Guid Id);

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/orders/{id}", async (string id, ISender sender) =>
		{
			if (!Guid.TryParse(id, out var guid))
			{
				throw new BadRequestException("Invalid order ID format. Please provide a valid GUID.");
			}
			var result = await sender.Send(new DeleteOrderCommand(guid));
			var response = result.Adapt<DeleteOrderResponse>();
			return Results.Ok(response);
		})
		.WithName("DeleteOrder")
		.Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithSummary("Delete Order")
		.WithDescription("Delete Order");
	}
}