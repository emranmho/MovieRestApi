using System.Data;
using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application.Validator;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;
    
    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.Genres)
            .NotEmpty();
        
        RuleFor(x=>x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists");
    }
    
    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken cancellationToken = default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug, null, cancellationToken);
        if(existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }
        
        return existingMovie is null;
    }
}