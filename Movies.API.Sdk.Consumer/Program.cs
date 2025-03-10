using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.API.Sdk;
using Movies.API.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;

// var moviesApi = RestService.For<IMoviesApi>("https://localhost:5001");

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>()
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("https://localhost:5001"))
    .ConfigureHttpClient(client =>
    {
        var serviceProvider = services.BuildServiceProvider();
        var authProvider = serviceProvider.GetRequiredService<AuthTokenProvider>();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authProvider.GetTokenAsync().Result);
    });

    
    
var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMovieAsync("1ffd3483-a846-4118-8863-0d899c66345a");
Console.WriteLine(JsonSerializer.Serialize(movie));
Console.WriteLine("--------------");
var request = new GetAllMoviesRequest
{
    Title = null,
    YearOfRelease = null,
    SortBy = null,
    Page = 1,
    PageSize = 3
};

var movies = await moviesApi.GetMoviesAsync(request);

foreach (var movieResponse in movies.Movies)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}

Console.WriteLine(JsonSerializer.Serialize(movies));