using System.Data.Common;
using Dapper;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Repositories;

namespace Modules.Users.Infrastructure.Repositories;

public class UnverifiedUserRepository(
    UserDbContext userDbContext,
    IDbConnectionFactory dbConnectionFactory) : IUnverifiedUserRepository
{
    public void Create(UnverifiedUser UnverifiedUser)
    {
        userDbContext.Add(UnverifiedUser);
    }

    public async Task<UnverifiedUser?> GetById(Guid id)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.CreateSqlConnection();
        string sqlQuery =
        $"""
        SELECT
        * 
        FROM 
        unverifiedUsers
        WHERE
        id = @id
        """;
        return await dbConnection.QueryFirstOrDefaultAsync<UnverifiedUser>(sqlQuery, new { id });
    }

}
