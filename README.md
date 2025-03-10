# MovieRestApi - Auth Branch

This branch builds on the [RealDatabaseData](https://github.com/emranmho/MovieRestApi/tree/RealDatabaseData) branch by adding authentication and authorization to the API.

## What's Included

- JWT token-based authentication
- Fake identity service for demonstration purposes
- Token validation middleware
- Claims-based authorization
- Policy-based authorization
- Protected endpoints

## Authentication vs Authorization

**Authentication**: Verifies the identity of a user or client making the request (who you are).

**Authorization**: Determines what resources a user has access to and what actions they can perform (what you can do).

## How JWT Works in REST APIs

1. Client sends credentials (username/password) to the authentication endpoint
2. Server validates credentials and returns a JWT token
3. Client includes this token in the Authorization header for subsequent requests
4. Server validates the token and authorizes the request based on claims

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

# Checkout the Auth branch
git checkout Auth
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

## Authentication Flow

1. Generate a token using the `/token` endpoint:
   ```
   POST /token
   {
       "userid": "d8566de3-b1a6-4a9b-b842-8e3887a82e42",
       "email": "emran@mhoemran.com",
       "customClaims": {
            "admin": true,
            "trusted_member": true
       }
   }
   ```

## Next Steps

Check out the [Ratings](https://github.com/emranmho/MovieRestApi/tree/Ratings) branch to see how to implement movie ratings functionality.