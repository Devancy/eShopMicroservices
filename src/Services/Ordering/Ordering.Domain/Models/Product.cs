namespace Ordering.Domain.Models;

public class Product : Entity<ProductId>
{
	public string Name { get; private set; } = null!;
	public decimal Price { get; private set; }
	
	public static Product Create(ProductId id, string name, decimal price)
	{
		ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
		ArgumentOutOfRangeException.ThrowIfNegative(price);
		
		return new Product{ Id = id, Name = name, Price = price };
	}
}