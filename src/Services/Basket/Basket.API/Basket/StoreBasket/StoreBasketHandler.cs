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

public class StoreBasketCommandHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
	public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
	{
		var cart = command.Cart;
		await repository.StoreBasket(cart, cancellationToken);
		// TODO: upsert the cache
		
		return new StoreBasketResult(cart.UserName, true);
	}
}