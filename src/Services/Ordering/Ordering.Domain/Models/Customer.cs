namespace Ordering.Domain.Models;

public class Customer : Entity<CustomerId>
{
	public string Name { get; private set; } = null!;
	public string Email { get; private set; } = null!;

	public static Customer Create(CustomerId id, string name, string email)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);
		ArgumentException.ThrowIfNullOrEmpty(email);

		var customer = new Customer
		{
			Id = id,
			Name = name,
			Email = email
		};
		
		return customer;
	}
}