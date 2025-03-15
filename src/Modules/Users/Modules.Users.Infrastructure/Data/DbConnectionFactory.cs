using System.Data;
using Microsoft.Data.SqlClient;
using Modules.Users.Application.Abstractions;

namespace Modules.Users.Infrastructure.Data;

public class DbConnectionFactory(string ConnectionString) : IDbConnectionFactory
{
    public IDbConnection CreateSqlConnection()
    {
        return new SqlConnection(ConnectionString);
    }

}
