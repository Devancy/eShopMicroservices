namespace Ordering.Domain.ValueObjects;

public record Address(
	string FirstName,
	string LastName,
	string EmailAddress, // Implicitly non-nullable
	string AddressLine, // Implicitly non-nullable
	string? Country,
	string? State,
	string? ZipCode)
{
	public static Address Of(
		string? firstName,
		string? lastName,
		string emailAddress,
		string addressLine,
		string? country,
		string? state,
		string? zipCode)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress, nameof(emailAddress));
		ArgumentException.ThrowIfNullOrWhiteSpace(addressLine, nameof(addressLine));

		return new Address(
			firstName ?? string.Empty,
			lastName ?? string.Empty,
			emailAddress,
			addressLine,
			country,
			state,
			zipCode);
	}
}