using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository(IDbConnectionFactory dbConnectionFactory) : IMovieRepository
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Movies (Id, Slug, Title, YearOfRelease)
            VALUES (@Id, @Slug, @Title, @YearOfRelease)
        """, movie, cancellationToken: token));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO Genres (MovieId, Name)
                    VALUES (@MovieId, @Name)
                """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
            }
        }
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
            SELECT m.*, string_agg(g.Name, ',') as Genres 
            FROM Movies m
            left join Genres g on m.Id = g.MovieId
            group by m.Id
        """, cancellationToken: token));

        var x = result.Select(row => new Movie
        {
            Id = row.id,
            Title = row.title,
            YearOfRelease = row.yearofrelease,
            Genres = Enumerable.ToList(row.genres.Split(","))
        });

        return x;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
            SELECT * FROM Movies WHERE Id = @Id
            """, new { id }, cancellationToken: token));
        
        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            SELECT Name FROM Genres WHERE MovieId = @Id
            """, new { id }, cancellationToken: token));
        
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        
        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
              SELECT * FROM Movies WHERE Slug = @Slug
              """, new { slug }, cancellationToken: token));
        
        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
              SELECT Name FROM Genres WHERE MovieId = @Id
              """, new { Id = movie.Id }, cancellationToken: token));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        
        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            Delete FROM Genres WHERE MovieId = @Id
            """, new {id = movie.Id}, cancellationToken: token));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO Genres (MovieId, Name)
                VALUES (@MovieId, @Name)
            """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token));
        }
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE Movies
            SET Slug = @Slug, Title = @Title, YearOfRelease = @YearOfRelease
            WHERE Id = @Id
        """, movie, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        
        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Genres WHERE MovieId = @Id
            """, new { id }, cancellationToken: token));
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Movies WHERE Id = @Id
            """, new { id }, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistByIdAsync(Guid id, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("""
            SELECT COUNT(1) FROM Movies WHERE Id = @Id
            """, new { id }, cancellationToken: token));
    }
}