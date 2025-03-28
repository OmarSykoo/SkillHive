using System.Net.Http.Headers;
using Dapper;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain;

namespace Modules.Users.Infrastructure;

public class UserRepository(UserDbContext userDbContext, IDbConnectionFactory dbConnectionFactory) : IUserRepository
{
    public async Task<Guid> CreateUser(User user)
    {
        await userDbContext.Users.AddAsync(user);
        return user.id;
    }

    public async Task<User?> GetUserByEmail(string Email, bool verified)
    {
        var sqlConnection = dbConnectionFactory.CreateSqlConnection();
        string sqlQuery =
        """
        SELECT
        *
        FROM 
        Users
        WHERE
        Email = @Email
        AND Verified = @verified
        """;
        User? user = await sqlConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Email, verified });
        return user;
    }

    public async Task<User?> GetUserById(Guid id, bool verified)
    {
        var sqlConnection = dbConnectionFactory.CreateSqlConnection();
        string sqlQuery =
        """
        SELECT
        * 
        FROM
        Users
        WHERE
        id = @id 
        AND Verified = @verified
        """;
        User? user = await sqlConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { id, verified });
        return user;
    }

    public async Task UpdateUser(User user)
    {
        await userDbContext.Users.AddAsync(user);
    }

}
