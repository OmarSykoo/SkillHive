using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Users.Application.UseCases.GetUserById;
using Modules.Users.Domain;
using Modules.Users.Domain.Exceptions;


namespace Modules.Users.Application.UseCases;

public sealed class UpdateUserNameCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserNameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetUserById(request.UserId);
        if (user is null)
            return new UserNotFound(request.UserId);
        user.UpdateName(request.FirstName, request.LastName);
        await userRepository.UpdateUser(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return request.UserId;
    }
}
