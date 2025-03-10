namespace Movies.Contracts.Responses;

public class PageResponse<TResponse>
{
    public required IEnumerable<TResponse> Movies { get; init; } = Enumerable.Empty<TResponse>();

    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
    public bool HasNextPage => Total > (Page * PageSize);
}