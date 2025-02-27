namespace Catalog.API.Exceptions;

public class ProductNotFoundException(Guid id) : Exception($"Product not found: '{id}'");