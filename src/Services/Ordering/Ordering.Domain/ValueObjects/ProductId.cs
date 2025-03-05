namespace Ordering.Domain.ValueObjects;

public record ProductId(Guid Value)
{
	public static ProductId Of(Guid value) =>
		// value == null ? throw new ArgumentNullException(nameof(value)) :
		value == Guid.Empty ? throw new DomainException("CustomerId cannot be empty.") :
		new ProductId(value);
}