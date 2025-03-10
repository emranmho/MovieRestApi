# Movie RestApi - BasicAPI Implementation

This branch implements a fundamental REST API for movie management using ASP.NET Core with in memory database.

## Overview

The `BasicApi` branch provides a straightforward implementation of a movie management API with basic CRUD operations using in memory database. This serves as the foundation for more advanced features in other branches. Added a Postman collection file (Rest API collection.postman_collection.json) for testing the API endpoints.

## Setup Instructions

### Prerequisites

- .NET SDK 8.0 or higher
- Git
- Postman

### Steps to Run

1. Clone the repository and switch to this branch:
   ```bash
   git clone https://github.com/emranmho/MovieRestApi.git
   cd MovieRestApi
   git checkout BasicApi
   ```
   
2. Run the application:
   ```bash
   dotnet run
   ```

3. The API will be available at:
    - https://localhost:5001
    - http://localhost:5000

## Key Features

- **Movie Entity**: Includes properties like Title, ReleaseYear, Genre, etc.
- **CRUD Operations**:
    - Create new movies via POST endpoint
    - Retrieve movie(s) via GET endpoints
    - Update existing movies via PUT endpoint
    - Delete movies via DELETE endpoint

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/movies | Retrieve all movies |
| GET | /api/movies/{id} | Retrieve a specific movie by ID |
| POST | /api/movies | Create a new movie |
| PUT | /api/movies/{id} | Update an existing movie |
| DELETE | /api/movies/{id} | Delete a movie |

## Postman Collection

A Postman collection (**Rest API collection.postman_collection.json**) is included in this repository to help you test the API. The collection contains requests for all CRUD operations.

## Next Steps

In the next section, we will move to reading from a database with Dapper and PostgreSQL for more persistent data storage.


