
using System.Data;

namespace Modules.Courses.Application.Abstractions
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateSqlConnection();
    }
}

