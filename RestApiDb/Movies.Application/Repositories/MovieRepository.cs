using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository(IDbConnectionFactory dbConnectionFactory) : IMovieRepository
{
    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Movies (Id, Slug, Title, YearOfRelease)
            VALUES (@Id, @Slug, @Title, @YearOfRelease)
        """, movie));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO Genres (MovieId, Name)
                    VALUES (@MovieId, @Name)
                """, new { MovieId = movie.Id, Name = genre }));
            }
        }
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition("""
            SELECT m.*, string_agg(g.Name, ',') as Genres 
            FROM Movies m
            left join Genres g on m.Id = g.MovieId
            group by m.Id
        """));

        var x = result.Select(row => new Movie
        {
            Id = row.id,
            Title = row.title,
            YearOfRelease = row.yearofrelease,
            Genres = Enumerable.ToList(row.genres.Split(","))
        });

        return x;
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
            SELECT * FROM Movies WHERE Id = @Id
            """, new { id }));
        
        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
            SELECT Name FROM Genres WHERE MovieId = @Id
            """, new { id }));
        
        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        
        return movie;
    }

    public async Task<Movie?> GetBySlugsync(string slug)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
              SELECT * FROM Movies WHERE Slug = @Slug
              """, new { slug }));
        
        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
              SELECT Name FROM Genres WHERE MovieId = @Id
              """, new { Id = movie.Id }));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }
        
        return movie;
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
            Delete FROM Genres WHERE MovieId = @Id
            """, new {id = movie.Id}));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO Genres (MovieId, Name)
                VALUES (@MovieId, @Name)
            """, new { MovieId = movie.Id, Name = genre }));
        }
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE Movies
            SET Slug = @Slug, Title = @Title, YearOfRelease = @YearOfRelease
            WHERE Id = @Id
        """, movie));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        
        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Genres WHERE MovieId = @Id
            """, new { id }));
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Movies WHERE Id = @Id
            """, new { id }));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistByIdAsync(Guid id)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("""
            SELECT COUNT(1) FROM Movies WHERE Id = @Id
            """, new { id }));
    }
}