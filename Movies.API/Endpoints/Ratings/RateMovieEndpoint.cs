using Microsoft.AspNetCore.OutputCaching;
using Movies.API.Auth;
using Movies.API.Endpoints.Movies;
using Movies.API.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Endpoints.Ratings;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovie";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Rate, async (
            Guid id, 
            RateMovieRequest request,
            IRatingService ratingService,
            HttpContext context,
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var result = await ratingService.RateMovieAsync(id, request.Rating, userId.Value, token);
            return result ? TypedResults.Ok() : Results.NotFound();
        })
        .WithName(Name)
        .RequireAuthorization();
        
        return app;
    }
}