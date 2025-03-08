
namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest(Guid Id);
// public record class DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (string id, ISender sender) =>
        {
            if (!Guid.TryParse(id, out var guid))
            {
                throw new BadRequestException("Invalid product ID format. Please provide a valid GUID.");
            }
            var result = await sender.Send(new DeleteProductCommand(guid));
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
