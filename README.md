# MovieRestApi - MinimalApi Branch

This branch builds on the [SDK](https://github.com/emranmho/MovieRestApi/tree/SDK) branch by reimplementing the API using the minimal API approach introduced in .NET 6.

## What's Included

- Minimal API implementation of the movie API
- Endpoint handlers without controllers
- Dependency injection with minimal APIs
- Authentication/authorization in minimal APIs
- Group-based endpoint organization

## What are Minimal APIs?

Minimal APIs are a simplified approach to building HTTP APIs with ASP.NET Core. They use a more compact syntax that requires less boilerplate code compared to the traditional controller-based approach.

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

# Checkout the MinimalApi branch
git checkout MinimalApi
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

## Minimal API Structure

The minimal API is structured around endpoint groups:

```csharp
app.MapGet(ApiEndpoints.Movies.GetAll, async (
            [AsParameters] GetAllMoviesRequest request,
            IMovieService movieService,
            HttpContext context,
            CancellationToken token) =>
        {
            var userId = context.GetUserId();
            var options = request.MapToOptions()
                .WithUser(userId);
            var movies = await movieService.GetAllAsync(options, token);
            var movieCount = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
            var response = movies.MapToMoviesResponse(
                request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                movieCount);
            return TypedResults.Ok(response);
        })
        .WithName($"{Name}V1")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .CacheOutput("MovieCache");

// Additional endpoints...
```

## Feature Comparison with Controller-Based API

| Feature | Controller-Based | Minimal API |
|---------|-----------------|------------|
| Code Size | More verbose | More concise |
| Organization | Controller classes | Endpoint groups |
| Filters | Attribute-based | Lambda-based |
| Dependency Injection | Constructor injection | Parameter injection |
| Model Binding | Automatic | Explicit |
| Documentation | XML comments | Inline documentation |

## Next Steps

Check out the [Main](https://github.com/emranmho/MovieRestApi/tree/main) branch to see the complete implementation with all features.