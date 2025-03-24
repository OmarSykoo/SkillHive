using FluentValidation;
using MassTransit.SagaStateMachine;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Common.Domain.Exceptions;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Exceptions;
using Modules.Users.Domain.Repositories;

namespace Modules.Users.Application.UseCases.VerifyEmail;

public record VerifyEmailCommand(string VerifyToken) : ICommand;

public sealed class VerifyEmailCommandHandler(
    IEmailVerificationTokenRepository emailVerificationTokenRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<VerifyEmailCommand>
{
    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        EmailVerificationToken? emailVerificationToken = await emailVerificationTokenRepository.GetByToken(request.VerifyToken);
        if (emailVerificationToken is null)
            return new TokenNotFound(request.VerifyToken);
        User? user = await userRepository.GetUserById(emailVerificationToken.UserId);
        if (user is null)
            return new UserNotFound(emailVerificationToken.UserId);
        user.Verify();
        await userRepository.UpdateUser(user);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}

internal sealed class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.VerifyToken).NotEmpty().Length(36);
    }
}