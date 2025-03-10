# MovieRestApi - AdvancedFeatures Branch

This branch builds on the [Ratings](https://github.com/emranmho/MovieRestApi/tree/Ratings) branch by adding advanced API features.

## What's Included

- **Filtering**: Query movies by various criteria (title, year, etc.)
- **Sorting**: Order results by different fields and directions
- **Pagination**: Limit the number of results per request
- **API Versioning**: Support for multiple API versions
- **Health Checks**: Monitor API health and dependencies
- **Output Caching**: Improve performance with response caching
- **API Key Authentication**: Alternative authentication method
- **Multiple Authentication Methods**: Support for both JWT and API Key authentication

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

# Checkout the AdvancedFeatures branch
git checkout AdvancedFeatures
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


## Feature Details

### Filtering

Filter movies using query parameters:
```
GET /api/movies?year=2025&title=hello
```

### Sorting

Sort results using query parameters:
```
GET /api/movies?sortby=-title
```

### Pagination

Paginate results using query parameters:
```
GET /api/movies?page=3&pageSize=2
```

### API Versioning

Access different API versions:
```
GET /api/movies?api-version=1.0
GET /api/movies?api-version=2.0
```

Or using headers:
```
X-API-Version: 1.0
```

### Health Checks

Check API health:
```
GET /health
```

### Output Caching

Responses are cached based on configuration for improved performance.

### API Key Authentication

Use API keys in the header:
```
X-API-Key: your-api-key
```

### Multiple Authentication

The API supports both JWT Bearer token and API Key authentication methods.

## Next Steps

Check out the [SDK](https://github.com/emranmho/MovieRestApi/tree/SDK) branch to see how to implement a client SDK using Refit.