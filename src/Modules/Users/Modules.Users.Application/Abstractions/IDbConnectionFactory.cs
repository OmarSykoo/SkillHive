
using System.Data;

namespace Modules.Users.Application.Abstractions
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateSqlConnection();
    }
}

