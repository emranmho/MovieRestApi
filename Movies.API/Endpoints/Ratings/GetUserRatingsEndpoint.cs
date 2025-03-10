using Movies.API.Auth;
using Movies.API.Mapping;
using Movies.Application.Services;

namespace Movies.API.Endpoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Ratings.GetUserRatings, async (
            IRatingService ratingService,
            HttpContext context,
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var ratings = await ratingService.GetRatingsForUserAsync(userId.Value, token);
            var ratingsResponse = ratings.MapToResponse();
            return TypedResults.Ok(ratingsResponse);
        })
        .WithName(Name)
        .RequireAuthorization();
        
        return app;
    }
}