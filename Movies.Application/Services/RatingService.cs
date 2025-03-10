using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class RatingService(
    IRatingRepository ratingRepository,
    IMovieRepository movieRepository
    ) : IRatingService
{
    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        if (rating is <= 0 or > 5)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure
                {
                    PropertyName = nameof(rating),
                    ErrorMessage = $"{nameof(rating)} must be between 0 and 5"
                }
            });
        }
        
        var movieExist = await movieRepository.ExistByIdAsync(movieId, token);
        if (!movieExist)
        {
            return false;
        }
        
        return await ratingRepository.RateMovieAsync(movieId, rating, userId, token);
    }

    public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        return await ratingRepository.DeleteRatingAsync(movieId, userId, token);
    }

    public Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        return ratingRepository.GetRatingsForUserAsync(userId, token);
    }
}