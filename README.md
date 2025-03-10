# MovieRestApi - RealDatabaseData Branch

This branch builds on the [BasicApi](https://github.com/emranmho/MovieRestApi/tree/BasicApi) branch by replacing the in-memory database with PostgreSQL and using Dapper for data access.

## What's Included

- PostgreSQL database integration
- Dapper for data access and ORM
- Docker Compose setup for PostgreSQL
- Database migrations and seeding
- Repository pattern implementation

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

# Checkout the RealDatabaseData branch
git checkout RealDatabaseData
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

## Database Configuration

The database connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=moviedb;Username=postgres;Password=postgres"
}
```

## Docker Compose Configuration

The `docker-compose.yml` file sets up PostgreSQL:

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:14
    container_name: movie-postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: moviedb
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql/data

volumes:
  postgres-data:
```

## Next Steps

Check out the [Auth](https://github.com/emranmho/MovieRestApi/tree/Auth) branch to see how to implement authentication and authorization.