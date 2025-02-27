using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Repositories;

namespace Movies.Application;

public static class AddApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}