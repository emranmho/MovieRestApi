using Microsoft.AspNetCore.Mvc;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers;

[ApiController]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await movieRepository.CreateAsync(movie);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }
    
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id) 
            ? await movieRepository.GetByIdAsync(id) 
            : await movieRepository.GetBySlugsync(idOrSlug);
        if(movie is null)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await movieRepository.GetAllAsync();
        var response = movies.MapToMoviesResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await movieRepository.UpdateAsync(movie);
        if(!updated)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
    {
        var deleted = await movieRepository.DeleteByIdAsync(id);
        if(!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
    
}