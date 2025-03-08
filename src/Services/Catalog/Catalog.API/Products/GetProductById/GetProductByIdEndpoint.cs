namespace Catalog.API.Products.GetProductById;

// public record GetProductByIdRequest();

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (string id, ISender sender) =>
        {
            if (!Guid.TryParse(id, out var guid))
            {
                throw new BadRequestException("Invalid product ID format. Please provide a valid GUID.");
            }
            var query = new GetProductByIdQuery(guid);
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id"); ;
    }
}