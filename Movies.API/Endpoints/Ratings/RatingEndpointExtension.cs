namespace Movies.API.Endpoints.Ratings;

public static class RatingEndpointExtension
{
    public static IEndpointRouteBuilder RatingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDeleteRating();
        app.MapRateMovie();
        app.MapGetUserRatings();
        return app;
    }
}