using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
	public CheckoutBasketCommandValidator()
	{
		RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckout can't be null");
		RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
	}
}

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint)
	: ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
	public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
	{
		var basket = await repository.GetBasket(command.BasketCheckout.UserName, cancellationToken);
		// if (basket is null)
		// {
		// 	return new CheckoutBasketResult(false);
		// }

		var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutEvent>();
		eventMessage.TotalPrice = basket.TotalPrice;

		await publishEndpoint.Publish(eventMessage, cancellationToken);

		await repository.RemoveBasket(command.BasketCheckout.UserName, cancellationToken);

		return new CheckoutBasketResult(true);
	}
}