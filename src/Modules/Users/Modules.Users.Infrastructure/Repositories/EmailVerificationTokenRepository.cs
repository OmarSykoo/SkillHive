using System.Data;
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
        IDbConnection connection = dbConnectionFactory.CreateSqlConnection();
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

}
