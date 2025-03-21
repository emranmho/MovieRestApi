using Microsoft.AspNetCore.OutputCaching;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.API.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (
            Guid id,
            UpdateMovieRequest request,
            IMovieService movieService,
            HttpContext context,
            IOutputCacheStore outputCacheStore,
            CancellationToken token) =>
        {
            var userId = context.GetUserId();

            var movie = request.MapToMovie(id);
            var updatedMovie = await movieService.UpdateAsync(movie, userId, token);
            if(updatedMovie is null)
            {
                return Results.NotFound();
            }
            var response = movie.MapToMovieResponse();
            await outputCacheStore.EvictByTagAsync("movies", token);
            return TypedResults.Ok(response);
        })
        .WithName(Name)
        .RequireAuthorization(AuthConstant.AdminUserPolicyName);
        
        return app;
    }
}