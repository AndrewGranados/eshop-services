using Catalog.Api.Common.Caching;
using Catalog.Api.Common.Pagination;

namespace Catalog.Api.Models.GetProducts
{
    public record GetProductsQuery(int PageNumber=1, int PageSize = 10) : IQuery<GetProductsResult>, ICacheableQuery
    {
        public string CacheKey => $"products-page-{PageNumber}-size-{PageSize}";
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }

    public record GetProductsResult(PaginatedResult<Product> Products);

    internal class GetProductsQueryHandler 
        (IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler.Hanle llamado {@query}", query);

            var totalCount = await session.Query<Product>()
                .LongCountAsync(cancellationToken);

            //de acuerdo con la paginación traemos los productos
            var products = await session.Query<Product>()
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var paginatedResult = new PaginatedResult<Product>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                Data = products
            };

            return new GetProductsResult(paginatedResult);
        }
    }
}
