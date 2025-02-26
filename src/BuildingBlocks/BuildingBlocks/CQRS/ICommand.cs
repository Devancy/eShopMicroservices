using MediatR;

namespace BuildingBlocks.CQRS;

// No-response command, Unit is void in MediatR
public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}