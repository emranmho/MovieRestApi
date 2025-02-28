using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers;

[ApiController]
[Authorize]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    [Authorize(AuthConstant.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie, token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }
    
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var movie = Guid.TryParse(idOrSlug, out var id) 
            ? await movieService.GetByIdAsync(id, token) 
            : await movieService.GetBySlugAsync(idOrSlug, token);
        if(movie is null)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAllMovies(CancellationToken token)
    {
        var movies = await movieService.GetAllAsync(token);
        var response = movies.MapToMoviesResponse();
        return Ok(response);
    }
    
    [Authorize(AuthConstant.AdminUserPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie, token);
        if(updatedMovie is null)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
    [Authorize(AuthConstant.TrustedMemberPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await movieService.DeleteByIdAsync(id, token);
        if(!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
    
}