namespace Movies.API.Endpoints.Movies;

public static class MovieEndpointExtension
{
    public static IEndpointRouteBuilder MapMovieEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGetMovie();
        app.MapCreateMovie();
        app.MapGetAllMovies();
        app.MapUpdateMovie();
        app.MapDeleteMovie();
        return app;
    }
}