


# MovieRestApi - BasicApi Branch

This branch implements a simple Movie CRUD API using an in-memory database.

## What's Included

- Basic CRUD operations for movies (Create, Read, Update, Delete)
- In-memory database for data storage
- RESTful API design principles

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer
- [Postman](https://www.postman.com/downloads/) or similar tool for API testing

## How to Clone

```bash
# Clone the repository
git clone https://github.com/emranmho/MovieRestApi.git

# Navigate to the repository folder
cd MovieRestApi

# Checkout the BasicApi branch
git checkout BasicApi
```

## Installation

1. Navigate to the project directory and restore dependencies:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access the API at `https://localhost:5001` or `http://localhost:5000`

## API Endpoints

| Method | Endpoint         | Description                     |
|--------|------------------|---------------------------------|
| GET    | /api/movies      | Get all movies                  |
| GET    | /api/movies/{id} | Get a movie by its ID           |
| POST   | /api/movies      | Create a new movie              |
| PUT    | /api/movies/{id} | Update an existing movie        |
| DELETE | /api/movies/{id} | Delete a movie                  |

## Next Steps

Check out the [RealDatabaseData](https://github.com/emranmho/MovieRestApi/tree/RealDatabaseData) branch to see how to implement PostgreSQL database integration with Dapper.