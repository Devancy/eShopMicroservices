namespace Ordering.Domain.ValueObjects;

public record OrderItemId(Guid Value)
{
    public static OrderItemId Of(Guid value) =>
        // value == null ? throw new ArgumentNullException(nameof(value)) :
        value == Guid.Empty ? throw new DomainException("OrderItemId cannot be empty.") :
        new OrderItemId(value);
}