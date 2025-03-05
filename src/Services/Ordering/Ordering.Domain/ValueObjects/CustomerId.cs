namespace Ordering.Domain.ValueObjects;

public record CustomerId(Guid Value)
{
	public static CustomerId Of(Guid value) =>
		// value == null ? throw new ArgumentNullException(nameof(value)) :
		value == Guid.Empty ? throw new DomainException("CustomerId cannot be empty.") :
		new CustomerId(value);
}