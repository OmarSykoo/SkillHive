using Modules.Common.Application.Messaging;

namespace Modules.Users.Application.UseCases;

public record UpdateUserNameCommand(Guid UserId, string UserName) : ICommand<Guid>;
