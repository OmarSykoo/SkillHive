using Modules.Common.Application.Messaging;
using Modules.Users.Domain.Exceptions;

namespace Modules.Users.Application.UseCases.CreateUser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <param name="Role"></param>
    /// <param name="Email"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="state"></param>
    /// <param name="city"></param>
    /// <param name="locationDesc"></param>
    /// <exception cref="UserConflictUserName"></exception>
    /// <exception cref="UserConflictEmail"></exception>
    public sealed record CreateUserCommand(string UserName, string Password, string Role, string Email, string PhoneNumber, string state, string city, string locationDesc) : ICommand<Guid>;
}

