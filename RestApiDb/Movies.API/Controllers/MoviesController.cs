using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers;

[ApiController]
// [Authorize]
[ApiVersion(1.0)]
[ApiVersion(2.0)]
public class MoviesController(
    IMovieService movieService,
    IOutputCacheStore outputCacheStore) 
    : ControllerBase
{
    [Authorize(AuthConstant.TrustedMemberPolicyName)]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> CreateMovie([FromBody]CreateMovieRequest request,
        CancellationToken token)
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie, token);
        await outputCacheStore.EvictByTagAsync("movies", token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }
    
    //just for quick bulk import
    // [Authorize(AuthConstant.TrustedMemberPolicyName)]
    // [HttpPost(ApiEndpoints.Movies.CreateBulk)]
    // public async Task<IActionResult> CreateBulkMovie([FromBody] IList<CreateMovieRequest> request,
    //     CancellationToken token)
    // {
    //     foreach (var item in request)
    //     {
    //         var movie = item.MapToMovie();
    //         await movieService.CreateAsync(movie, token);
    //     }
    //     return NoContent();
    //
    // }
    
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var movie = Guid.TryParse(idOrSlug, out var id) 
            ? await movieService.GetByIdAsync(id, userId,token) 
            : await movieService.GetBySlugAsync(idOrSlug, userId,token);
        if(movie is null)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        return Ok(response);
    }
    
 
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    [MapToApiVersion(1.0)]
    [OutputCache(PolicyName = "MovieCache")]
    public async Task<IActionResult> GetAllMoviesV1(
        [FromQuery] GetAllMoviesRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);
        var movies = await movieService.GetAllAsync(options, token);
        var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
        var response = movies.MapToMoviesResponse(
            request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
            request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize), movieCount);
        return Ok(response);
    }
    
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    [MapToApiVersion(2.0)]
    public async Task<IActionResult> GetAllMoviesV2(
        [FromQuery] GetAllMoviesRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var options = request.MapToOptions()
            .WithUser(userId);
        var movies = await movieService.GetAllAsync(options, token);
        var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
        var response = movies.MapToMoviesResponse(
            request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
            request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize), 
            movieCount);
        return Ok(response);
    }


    
    [Authorize(AuthConstant.AdminUserPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, 
        [FromBody] UpdateMovieRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie, userId, token);
        if(updatedMovie is null)
        {
            return NotFound();
        }
        var response = movie.MapToMovieResponse();
        await outputCacheStore.EvictByTagAsync("movies", token);
        return Ok(response);
    }
    
    [Authorize(AuthConstant.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> DeleteMovie([FromRoute] Guid id,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var deleted = await movieService.DeleteByIdAsync(id, token);
        if(!deleted)
        {
            return NotFound();
        }
        await outputCacheStore.EvictByTagAsync("movies", token);
        return Ok();
    }
    
}