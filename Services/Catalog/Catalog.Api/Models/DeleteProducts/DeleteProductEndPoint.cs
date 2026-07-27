namespace Catalog.Api.Models.DeleteProducts
{
    //public record DeleteProductRequest(Guid Id)
    public record DeleteProductRenspose(bool IsSuccess);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));
                var response = result.Adapt<DeleteProductRenspose>();
                return Results.Ok(response);
            })
            .WithName("DeleteProgram")
            .Produces<DeleteProductRenspose>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Borrar producto")
            .WithDescription("Eliminar producto");
        }
    }
}
