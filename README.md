
# MovieRestApi - Ratings Branch

This branch builds on the [Auth](https://github.com/emranmho/MovieRestApi/tree/Auth) branch by adding movie ratings functionality.

## What's Included

- CRUD operations for movie ratings
- User-based rating system
- Rating aggregation and statistics
- Authorization for rating operations
- Relationship between users, movies, and ratings

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

# Checkout the Ratings branch
git checkout Ratings
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



## Rating System

- Users can rate movies on a scale from 1 to 5
- Each user can submit only one rating per movie
- Users can update or delete their ratings
- Movie details include average rating and rating count

## API Endpoints

| Method | Endpoint                      | Description                        |
|--------|-------------------------------|------------------------------------|
| GET    | /api/ratings/me               | Get all ratings for a movie        |
| PUT    | /api/movies/{id}/ratings      | Update a user's rating for a movie |
| DELETE | /api/movies/{id}/ratings      | Delete a user's rating for a movie |

## Next Steps

Check out the [AdvancedFeatures](https://github.com/emranmho/MovieRestApi/tree/AdvancedFeatures) branch to see how to implement advanced API features like filtering, pagination, and more.