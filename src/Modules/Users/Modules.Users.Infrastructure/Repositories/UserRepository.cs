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

    public async Task<User?> GetUserByEmail(string Email)
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
        """;
        User? user = await sqlConnection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Email });
        return user;
    }

    public async Task<User?> GetUserById(Guid id)
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
        """;
        User? user = await sqlConnection.QueryFirstOrDefaultAsync(sqlQuery, new { id });
        return user;
    }

    public async Task UpdateUser(User user)
    {
        await userDbContext.Users.AddAsync(user);
    }

}
