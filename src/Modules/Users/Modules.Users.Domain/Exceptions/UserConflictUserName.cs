using System.Globalization;
using Modules.Common.Domain.Exceptions;

namespace Modules.Users.Domain.Exceptions
{
    public class UserConflictUserName(string username) :
        ConflictException("User.Conflict.Username", $"User with this username {username} already exists ")
    {
    }
}

