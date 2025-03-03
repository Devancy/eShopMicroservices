using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName, bool IsSuccess);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
	public StoreBasketCommandValidator()
	{
		RuleFor(x => x.Cart).NotNull().WithMessage("Please provide a valid cart");
		RuleFor(x => x.Cart.UserName).NotNull().WithMessage("Please provide a valid username");
	}
}

public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProtoClient) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
	public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
	{
		await DeductDiscount(command.Cart, cancellationToken);
		await repository.StoreBasket(command.Cart, cancellationToken);
		// TODO: upsert the cache
		
		return new StoreBasketResult(command.Cart.UserName, true);
	}

	private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
	{
		foreach (var item in cart.Items)
		{
			var coupon = await discountProtoClient.GetDiscountAsync(new GetDiscountRequest
			{
				ProductName = item.ProductName,
			}, cancellationToken: cancellationToken);
			item.Price -= coupon.Amount;
		}
	}
}