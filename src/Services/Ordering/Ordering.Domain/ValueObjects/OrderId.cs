namespace Ordering.Domain.ValueObjects;

public record OrderId(Guid Value)
{
	public static OrderId Of(Guid value) =>
		// value == null ? throw new ArgumentNullException(nameof(value)) :
		value == Guid.Empty ? throw new DomainException("CustomerId cannot be empty.") :
		new OrderId(value);
}