namespace Basket.API.Basket.DeleteBasket;

//public record DeleteBasketRequest(string UserName);

public record DeleteBasketResponse(string UserName, bool IsSuccess);

public class DeleteBasketCommandEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
		{
			var result = await sender.Send(new DeleteBasketCommand(userName));
			var response = result.Adapt<DeleteBasketResponse>();
			
			if( response is { IsSuccess: true})
			{
				return Results.NoContent();
			}
			return Results.NotFound();
		})
		.WithName("DeleteBasket")
		.Produces<IResult>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.WithSummary("Delete Basket")
		.WithDescription("Delete Basket");
	}
}