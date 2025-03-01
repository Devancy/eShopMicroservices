namespace Catalog.API.Products.CreateProduct;

// ReSharper disable once ClassNeverInstantiated.Global
public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be from zero");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            //Id = Guid.NewGuid(), // TODO: remove this line later
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            Price = command.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}