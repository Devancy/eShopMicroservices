namespace Catalog.API.Products.GetProductByCategory;

public record class GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting product by {@Query}", query);
        ArgumentException.ThrowIfNullOrEmpty(query.Category, nameof(query.Category));

        var products = await session.Query<Product>().Where(p => p.Categories.Contains(query.Category)).ToListAsync(cancellationToken);
        if (products == null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByCategoryResult(products);
    }
}