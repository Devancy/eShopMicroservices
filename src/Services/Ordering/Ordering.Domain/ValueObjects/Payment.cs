namespace Ordering.Domain.ValueObjects;

public record Payment(
	string CardName,
	string CardNumber,
	string? Expiration,
	string CVV,
	int PaymentMethod)
{
	public static Payment Of(
		string cardName,
		string cardNumber,
		string? expiration,
		string cvv,
		int paymentMethod)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(cardName, nameof(cardName));
		ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
		ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3, nameof(cvv));
		ArgumentOutOfRangeException.ThrowIfLessThan(paymentMethod, 0, nameof(paymentMethod));

		return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
	}
}