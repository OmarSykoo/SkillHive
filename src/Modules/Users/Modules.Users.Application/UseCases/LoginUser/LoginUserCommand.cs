using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Common.Domain.Exceptions;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.UseCases.LoginUser;

public record LoginUserCommand(string email, string password) : IQuery<LoginUserCommandResponse>;
public class LoginUserCommandHandler(IUserRepository userRepository, IRefreshRepository refreshRepository, IJwtProvider jwtProvider, IUnitOfWork unitOfWork) : IQueryHandler<LoginUserCommand, LoginUserCommandResponse>
{
    public async Task<Result<LoginUserCommandResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetUserByEmail(request.email, true);
        if (user == null)
            return new NotFoundException("Email.NotFound", $"User with email {request.email} not found");
        var hasher = new PasswordHasher<Object>();
        if (hasher.VerifyHashedPassword(new object(), user.HashedPassword, request.password) == PasswordVerificationResult.Success)
        {
            var refreshToken = new RefreshToken()
            {
                ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
                Id = Guid.NewGuid(),
                Token = jwtProvider.GenerateReferesh(),
                UserId = user.id
            };
            refreshRepository.Create(refreshToken);
            await unitOfWork.SaveChangesAsync();
            return new LoginUserCommandResponse(refreshToken.Token, jwtProvider.GenerateAccesss(user));
        }
        return new NotAuthorizedException("Login.Password", $"Incorrect password");
    }
}
public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        RuleFor(x => x.password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
        .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
        .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
        .Matches(@"\d").WithMessage("Password must contain at least one number.")
        .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
    }
}
public record LoginUserCommandResponse(string refreshToken, string accessToken);
