using Microsoft.AspNetCore.OutputCaching;
using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Services;

namespace Movies.API.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.Delete, async (
            Guid id,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            CancellationToken token) =>
        {
            var deleted = await movieService.DeleteByIdAsync(id, token);
            if(!deleted)
            {
                return Results.NotFound();
            }
            await outputCacheStore.EvictByTagAsync("movies", token);
            return TypedResults.Ok();
        })
        .WithName(Name)
        .RequireAuthorization(AuthConstant.AdminUserPolicyName);
        
        return app;
    }
}