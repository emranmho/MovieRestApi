using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(
    IMovieRepository movieRepository,
    IValidator<Movie> movieValidator,
    IValidator<GetAllMoviesOptions> gtAllMoviesOptionsValidator,
    IRatingRepository ratingRepository)
    : IMovieService
{
   
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default )
    {
        await movieValidator.ValidateAndThrowAsync(movie, token);
        return await movieRepository.CreateAsync(movie, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default )
    {
        await gtAllMoviesOptionsValidator.ValidateAndThrowAsync(options, token);
        return await movieRepository.GetAllAsync(options, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userId, CancellationToken token = default )
    {
        return movieRepository.GetByIdAsync(id, userId, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userId, CancellationToken token = default )
    {
        return movieRepository.GetBySlugAsync(slug, userId, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId, CancellationToken token = default )
    {
        await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await movieRepository.ExistByIdAsync(movie.Id, token);
        if (!movieExists)
        {
            return null;
        }
        
        await movieRepository.UpdateAsync(movie, token);

        if (!userId.HasValue)
        {
            var rating = await ratingRepository.GetRatingAsync(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }
        
        var ratings = await ratingRepository.GetRatingAsync(movie.Id, userId.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;
        return movie;

    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default )
    {
        return movieRepository.DeleteByIdAsync(id, token);
    }
}