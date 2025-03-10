# MovieRestApi - SDK Branch

This branch builds on the [AdvancedFeatures](https://github.com/emranmho/MovieRestApi/tree/AdvancedFeatures) branch by implementing a client SDK using the Refit library.

## What's Included

- Client SDK using Refit library
- Type-safe HTTP client for the API
- Authentication handling in the SDK
- Request/response DTOs for API interaction
- Example usage of the SDK

## What is Refit?

Refit is a library for .NET that turns REST APIs into live interfaces. It simplifies API consumption by allowing you to define your API as C# interfaces, which Refit then implements for you.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer
- [Docker](https://www.docker.com/products/docker-desktop) for PostgreSQL
- [Postman](https://www.postman.com/downloads/) or similar tool for API testing

## How to Clone

```bash
# Clone the repository
git clone https://github.com/emranmho/MovieRestApi.git

# Navigate to the repository folder
cd MovieRestApi

# Checkout the SDK branch
git checkout SDK
```

## Installation

1. In the project directory, open cmd and run the command to start the PostgreSQL database using Docker:
   ```bash
   docker compose up 
   ```

2. Navigate to the project directory and restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Access the API at `https://localhost:5001` or `http://localhost:5000`

## SDK Usage Example

```csharp
// Register the SDK in your application
ervices
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

// Use the SDK in your services
public class MovieService
{
    private readonly IMovieApi _movieApi;

    public MovieService(IMovieApi movieApi)
    {
        _movieApi = movieApi;
    }

    var request = new GetAllMoviesRequest
    {
       Title = null,
       YearOfRelease = null,
       SortBy = null,
       Page = 1,
       PageSize = 3
    };
   
    var movies = await _movieApi.GetMoviesAsync(request);
   
    oreach (var movieResponse in movies.Movies)
    {
       Console.WriteLine(JsonSerializer.Serialize(movieResponse));
    }
}
```



## Next Steps

Check out the [MinimalApi](https://github.com/emranmho/MovieRestApi/tree/MinimalApi) branch to see how to implement the same API using the minimal API approach.