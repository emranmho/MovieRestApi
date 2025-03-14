using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Services;

namespace Movies.API.Endpoints.Movies;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";

    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.Get, async (
            string idOrSlug,
            IMovieService movieService,
            HttpContext context,
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var movie = Guid.TryParse(idOrSlug, out var id) 
                ? await movieService.GetByIdAsync(id, userId,token) 
                : await movieService.GetBySlugAsync(idOrSlug, userId,token);
            if(movie is null)
            {
                return Results.NotFound();
            }
            var response = movie.MapToMovieResponse();
            return TypedResults.Ok(response);
        })
        .WithName(Name);
        
        return app;
    }
}