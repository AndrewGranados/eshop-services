namespace Catalog.Api.Common.Pagination
{
    public record PaginationRequest(
        int PageNumber = 1,
        int PageSize = 10
    );
}
