namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(string UserName, bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
	public DeleteBasketCommandValidator()
	{
		RuleFor(command => command.UserName).NotEmpty().WithMessage("UserName is required");
	}
}

public class DeleteBasketCommandHandler(IBasketRepository repository) : IRequestHandler<DeleteBasketCommand, DeleteBasketResult>
{
	public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
	{
		await repository.RemoveBasket(command.UserName, cancellationToken);
		
		return new DeleteBasketResult(command.UserName, true);
	}
}