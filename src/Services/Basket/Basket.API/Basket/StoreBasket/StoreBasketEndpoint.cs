namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName, bool IsSuccess);

public class StoreBasketEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
		{
			var command = request.Adapt<StoreBasketCommand>();
			var result = await sender.Send(command);
			var response = result.Adapt<StoreBasketResponse>();
			
			return Results.Created($"/basket/{response.UserName}", null);
		})
		.WithName("CreateBasket")
		.Produces<IResult>(StatusCodes.Status201Created)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.WithSummary("Create Basket")
		.WithDescription("Create Basket");
	}
}