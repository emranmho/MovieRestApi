namespace Movies.Contracts.Requests;

public class GetAllMoviesRequest
{
    public string? Title { get; init; }

    public int? YearOfRelease { get; init; }
    public string? SortBy { get; init; }
}