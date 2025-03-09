using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Controllers;

[ApiController]
[ApiVersion(1.0)]

public class RatingsController(IRatingService ratingService) : ControllerBase
{
    [Authorize]
    [HttpPut(ApiEndpoints.Movies.Rate)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id, 
        [FromBody] RateMovieRequest request,
        CancellationToken token = default)
    {
        var userId = HttpContext.GetUserId();
        var result = await ratingService.RateMovieAsync(id, request.Rating, userId.Value, token);
        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
    public async Task<IActionResult> DeleteRate([FromRoute] Guid id, 
        CancellationToken token = default)
    {
        var userId = HttpContext.GetUserId();
        var result = await ratingService.DeleteRatingAsync(id, userId.Value, token);
        return result ? Ok() : NotFound();
    }
    
    [Authorize]
    [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> DeleteRate(CancellationToken token = default)
    {
        var userId = HttpContext.GetUserId();
        var ratings = await ratingService.GetRatingsForUserAsync(userId.Value, token);
        var ratingsResponse = ratings.MapToResponse();
        return Ok(ratingsResponse);
    }
}