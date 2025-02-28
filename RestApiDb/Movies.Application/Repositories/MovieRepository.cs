using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new();
    
    public Task<bool> CreateAsync(Movie movie)
    {
        _movies.Add(movie);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return Task.FromResult(_movies.AsEnumerable());
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        var movie = _movies.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(movie);
    }

    public Task<Movie?> GetBySlugsync(string idOrSlug)
    {
        var movie = _movies.FirstOrDefault(x => x.Slug == idOrSlug);
        return Task.FromResult(movie);
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        var index = _movies.FindIndex(x => x.Id == movie.Id);
        if (index == -1)
        {
            return Task.FromResult(false);
        }

        _movies[index] = movie;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var movie = _movies.FirstOrDefault(x => x.Id == id);
        if (movie == null)
        {
            return Task.FromResult(false);
        }

        _movies.Remove(movie);
        return Task.FromResult(true);
    }
}