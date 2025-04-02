using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Repositories;

namespace Modules.Users.Infrastructure.Repositories;

public class EmailVerificationTokenRepository(UserDbContext userDbContext, IDbConnectionFactory dbConnectionFactory) : IEmailVerificationTokenRepository
{
    public async Task Create(EmailVerificationToken emailVerificationToken)
    {
        await userDbContext.emailVerificationTokens.AddAsync(emailVerificationToken);
    }

    public async Task<EmailVerificationToken?> GetByToken(string Token)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateSqlConnection();
        string query =
        """
        SELECT
        * 
        FROM 
        EmailVerificationTokens
        WHERE
        Token = @Token
        """;
        EmailVerificationToken? verificationToken = await connection
            .QueryFirstOrDefaultAsync<EmailVerificationToken>(query, new { Token });
        return verificationToken;
    }

    public async Task<EmailVerificationToken?> GetByUserId(Guid id)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateSqlConnection();
        string query =
        $"""
        SELECT
        *
        FROM
        EmailVerificationTokens
        WHERE 
        {nameof(EmailVerificationToken.UserId)} = @UserId
        """;
        return await connection.QueryFirstOrDefaultAsync<EmailVerificationToken>(query, new { UserId = id });
    }
}
