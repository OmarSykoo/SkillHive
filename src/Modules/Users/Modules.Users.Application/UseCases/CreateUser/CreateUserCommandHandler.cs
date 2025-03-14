using Microsoft.AspNetCore.Identity;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Users.Domain;
using Modules.Users.Domain.Exceptions;


namespace Modules.Users.Application.UseCases.CreateUser
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await userRepository.GetUserByEmail(request.Email) is not null)
            {
                return new UserConflictEmail(request.Email);
            }
            if (await userRepository.GetUserByUserName(request.UserName) is not null)
            {
                return new UserConflictUserName(request.UserName);
            }
            PasswordHasher<object> passwordHasher = new PasswordHasher<object>();
            string HashedPassword = passwordHasher.HashPassword(new object(), request.Password);
            var user = User.Create(
                request.UserName,
                HashedPassword,
                request.Role,
                request.Email,
                request.PhoneNumber,
                request.state,
                request.city,
                request.locationDesc);
            await userRepository.CreateUser(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return user.id;
        }
    }

}

