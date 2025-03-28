using Microsoft.AspNetCore.Identity;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Users.Application.Services;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Exceptions;
using Modules.Users.Domain.Repositories;


namespace Modules.Users.Application.UseCases.CreateUser
{
    public class CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IEmailVerificationTokenRepository emailVerificationTokenRepository) : ICommandHandler<CreateUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await userRepository.GetUserByEmail(request.Email) is not null)
            {
                return new UserConflictEmail(request.Email);
            }
            PasswordHasher<object> passwordHasher = new PasswordHasher<object>();
            string HashedPassword = passwordHasher.HashPassword(new object(), request.Password);
            var user = User.Create(
                request.FirstName,
                request.LastName,
                HashedPassword,
                request.Role,
                request.Email,
                request.PhoneNumber,
                request.state,
                request.city,
                request.locationDesc);
            var emailVerificationToken = EmailVerificationToken.Create(user.id);
            await userRepository.CreateUser(user);
            await emailVerificationTokenRepository.Create(emailVerificationToken);
            await emailService.SendVerificationToken(user.FirstName, user.Email, emailVerificationToken.Token);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return user.id;
        }
    }

}

