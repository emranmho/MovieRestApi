
# MovieRestApi

A comprehensive REST API for movie management with various features including authentication, database integration, and advanced querying capabilities.

## What's Covered in This Repository

This repository demonstrates the progressive development of a Movie REST API with the following features:

- Basic CRUD operations for movies
- Database integration with PostgreSQL
- Authentication and authorization using JWT
- Movie ratings functionality
- Advanced features (filtering, pagination, caching, etc.)
- SDK implementation using Refit
- Minimal API implementation

## Branches

The repository is organized into several branches, each building on the previous one:

1. [BasicApi](https://github.com/emranmho/MovieRestApi/tree/BasicApi) - Simple movie CRUD operations with in-memory database
2. [RealDatabaseData](https://github.com/emranmho/MovieRestApi/tree/RealDatabaseData) - PostgreSQL integration with Dapper
3. [Auth](https://github.com/emranmho/MovieRestApi/tree/Auth) - JWT authentication and authorization
4. [Ratings](https://github.com/emranmho/MovieRestApi/tree/Ratings) - Movie ratings functionality
5. [AdvancedFeatures](https://github.com/emranmho/MovieRestApi/tree/AdvancedFeatures) - Advanced API features
6. [SDK](https://github.com/emranmho/MovieRestApi/tree/SDK) - SDK implementation with Refit
7. [MinimalApi](https://github.com/emranmho/MovieRestApi/tree/MinimalApi) - Minimal API implementation
8. [Main](https://github.com/emranmho/MovieRestApi/tree/main) - Current branch with all features

## Project Structure

## How to Clone

```bash
# Clone the repository
git clone https://github.com/emranmho/MovieRestApi.git

# Navigate to the repository folder
cd MovieRestApi

# If you want to checkout a specific branch
git checkout <branch-name>
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer
- [Docker](https://www.docker.com/products/docker-desktop) (for PostgreSQL)
- [Postman](https://www.postman.com/downloads/) or similar tool for API testing

## Installation

1. Clone the repository as described above
2. In the project directory, open cmd and run the command to start the PostgreSQL database using Docker:
   ```bash
   docker compose up 
   ```

3. Navigate to the project directory and restore dependencies:
   ```bash
   dotnet restore
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Access the API at `https://localhost:5001` or `http://localhost:5000`


## API Testing

A Postman collection is included in the repository to help you test all the API endpoints. You can find it in the project directory name `MovieRestApi.postman_collection`.

To use it:
1. Open Postman
2. Click on "Import" button
3. Select the `MovieRestApi.postman_collection` Postman collection file
4. The collection includes requests for all endpoints organized by feature

## Features Documentation

For detailed information about specific features, please refer to the README files in each branch.
