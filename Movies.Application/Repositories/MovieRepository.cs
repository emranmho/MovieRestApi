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

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

        var orderClause = string.Empty;

        if (options.SortField is not null)
        {
            orderClause = $"""
               , m.{options.SortField}
               order by m.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}
               """;
        }

        var result = await connection.QueryAsync(new CommandDefinition($"""
            SELECT m.*, 
                   string_agg(distinct g.Name, ',') as Genres ,
                   round(avg(r.rating), 1) as rating, 
                   myr.rating as userrating 
            FROM Movies m
            left join Genres g on m.Id = g.MovieId
            left join ratings r on m.Id = r.MovieId
            left join ratings myr on m.Id = myr.MovieId
                and myr.UserId = @userId
            where (@title is null or m.title like ('%' || @title || '%'))
            and (@yearofrelease is null or m.yearofrelease = @yearofrelease)
            group by m.Id,userrating {orderClause}
            limit @pageSize
            offset @pageOffset
        """, new
        {
            userId = options.UserId,
            title = options.Title,
            yearofrelease = options.YearOfRelease,
            pageSize = options.PageSize,
            pageOffset = (options.Page -1) * options.PageSize,
        }, cancellationToken: token));

        var x = result.Select(row => new Movie
        {
            Id = row.id,
            Title = row.title,
            YearOfRelease = row.yearofrelease,
            Rating = (float?)row.rating,
            UserRating = (int?)row.userrating,
            Genres = Enumerable.ToList(row.genres.Split(","))
        });

        return x;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
            SELECT m.*, 
                   round(avg(r.rating), 1) as rating, 
                   myr.rating as userrating 
            FROM Movies m
            left join ratings r on m.Id = r.MovieId
            left join ratings myr on m.Id = myr.MovieId
                and myr.UserId = @userId
            WHERE Id = @Id
            group by id, userrating
            """, new { id , userId}, cancellationToken: token));
        
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

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId, CancellationToken token = default )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await connection.QueryFirstOrDefaultAsync<Movie>(
            new CommandDefinition("""
              SELECT m.*, round(avg(r.rating), 1) as Rating, myr.rating as UserRating 
              FROM Movies m
              left join ratings r on m.Id = r.MovieId
              left join ratings myr on m.Id = myr.MovieId
                  and myr.UserId = @userId
              WHERE Slug = @Slug
              group by id, UserRating
              """, new { slug, userId}, cancellationToken: token));
        
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
        
        await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Ratings WHERE MovieId = @Id
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

    public async Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleAsync<int>(
            new CommandDefinition("""
              SELECT COUNT(id) 
              FROM Movies 
              WHERE (@title is null or title like ('%' || @title || '%'))
              and (@yearofrelease is null or yearofrelease = @yearofrelease)
              """, new
            {
                title, 
                yearOfRelease 
            }, cancellationToken: token));
    }
}