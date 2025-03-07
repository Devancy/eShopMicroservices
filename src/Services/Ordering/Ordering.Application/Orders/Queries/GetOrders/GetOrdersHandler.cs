namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
	public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
	{
		var pageIndex = query.PaginationRequest.PageIndex;
		var pageSize = query.PaginationRequest.PageSize;
		var total = await dbContext.Orders.LongCountAsync(cancellationToken);
		
		var orders = await dbContext.Orders
			.Include(o => o.OrderItems)
			.OrderBy(o => o.OrderName)
			.Skip(pageSize * pageIndex)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, total, orders.ToOrderDtoList()));
	}
}