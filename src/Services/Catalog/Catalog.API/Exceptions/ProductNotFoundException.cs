namespace Catalog.API.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException() : base("Product was not found.")
    {
        
    }

    public ProductNotFoundException(Guid id) : base($"Product with id {id} was not found.")
    {
        
    }
}