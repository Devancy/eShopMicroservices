namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(
	IEnumerable<Product> Products,
	long Count,
	long PageNumber,
	long PageSize,
	long PageCount,
	long TotalItemCount,
	bool HasPreviousPage,
	bool HasNextPage,
	bool IsFirstPage,
	bool IsLastPage);

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
	public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
	{
		var products = await session.Query<Product>()
			.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

		var result = products.Adapt<GetProductsResult>() with { Products = products };
		return result;
	}
}