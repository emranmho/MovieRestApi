using Dapper;

namespace Movies.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;
    
    public DbInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS Movies (
                Id UUID PRIMARY KEY,
                Slug TEXT NOT NULL,
                Title TEXT NOT NULL,
                YearOfRelease INT NOT NULL)
        """);
        
        await connection.ExecuteAsync("""
            create unique index concurrently if not exists movies_slug_idx
            on Movies
            using btree (slug)
        """);
      
        await connection.ExecuteAsync("""
            CREATE TABLE IF NOT EXISTS Genres (
                MovieId UUID references Movies(Id),
                Name TEXT NOT NULL)
        """);
    }
}