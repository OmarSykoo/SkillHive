using Modules.Common.Application.Messaging;
using Modules.Users.Domain.Exceptions;

namespace Modules.Users.Application.UseCases.CreateUser
{
    public sealed record CreateUserCommand(string UserName, string Password, string Role, string Email, string PhoneNumber, string state, string city, string locationDesc) : ICommand<Guid>;
}

