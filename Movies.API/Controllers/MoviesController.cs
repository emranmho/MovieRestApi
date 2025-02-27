using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers;

[ApiController]
[Route("api")]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase
{
    [HttpPost("movies")]
    public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };
        await movieRepository.CreateAsync(movie);
        return Ok(movie);
    }
    
}