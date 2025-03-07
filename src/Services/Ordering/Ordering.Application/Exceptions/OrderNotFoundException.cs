using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(Guid id) : NotFoundException("Order", id);