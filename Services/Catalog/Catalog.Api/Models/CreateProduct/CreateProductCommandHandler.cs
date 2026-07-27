using BuildingBlocks.CQRS;
using Marten;
using System.Windows.Input;

namespace Catalog.Api.Models.CreateProduct
{
    public record CreateProductCommand(string Name, string Descripcion, List<string> Category, string ImageFiles, decimal Price) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = request.Name,
                Descripcion = request.Descripcion,
                Category = request.Category,
                ImageFiles = request.ImageFiles,
                Price = request.Price
            };

            documentSession.Store(product);
            await documentSession.SaveChangesAsync(cancellationToken);   
            return new CreateProductResult(product.Id);
        }
    }
}
