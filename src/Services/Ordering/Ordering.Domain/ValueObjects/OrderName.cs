namespace Ordering.Domain.ValueObjects;

public record OrderName(string Value)
{
	private const int DefaultLength = 5;

	public static OrderName Of(string value) =>
		string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(nameof(value)) : new OrderName(value);
}