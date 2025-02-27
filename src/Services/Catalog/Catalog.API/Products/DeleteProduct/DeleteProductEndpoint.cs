
namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest(Guid Id);
// public record class DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            if( result is { IsSuccess: true})
            {
                return Results.NoContent();
            }
            return Results.NotFound();
        })
        .WithName("DeleteProduct")
        .Produces<IResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product"); ;
    }
}
