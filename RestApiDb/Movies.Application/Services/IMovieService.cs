using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie); 
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(Guid id);
    Task<Movie?> GetBySlugAsync(string slug);
    Task<Movie?> UpdateAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id);
}