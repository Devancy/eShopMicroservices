namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
	public UpdateOrderCommandValidator()
	{
		RuleFor(x => x.Order).NotNull().WithMessage("Order cannot be empty");
		RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required")
			.When(x => x.Order != null!);
		RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required")
			.When(x => x.Order != null!);
		RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required")
			.When(x => x.Order != null!);
	}
}