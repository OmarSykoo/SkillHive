using FluentValidation;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Common.Domain.Exceptions;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.UseCases.LoginUser;
using Modules.Users.Domain;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Exceptions;
using Modules.Users.Domain.Repositories;

namespace Modules.Users.Application.UseCases;

public record LoginWithFaceCommand(byte[] imgBinaries) : ICommand<LoginUserCommandResponse>;

public class LoginWithFaceCommandHandler(
    IFaceEmbedingRepository faceEmbedingRepository,
    IFaceModelService faceModelService,
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IRefreshRepository refreshRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LoginWithFaceCommand, LoginUserCommandResponse>
{
    public async Task<Result<LoginUserCommandResponse>> Handle(LoginWithFaceCommand request, CancellationToken cancellationToken)
    {
        ICollection<float> embeding = await faceModelService.FaceImgToEmbeding(request.imgBinaries);
        (float? score, Guid? user_id) = await faceEmbedingRepository.GetMatching(embeding);
        System.Console.WriteLine("------------------------");
        System.Console.WriteLine(score);
        System.Console.WriteLine(user_id);
        System.Console.WriteLine("------------------------");
        if (score == null && user_id == null)
            return new FaceNotFound();
        if (score < faceModelService.acceptanceValue)
        {
            return new FaceNotFound();
        }
        if (score == null || user_id == null)
            throw new SkillHiveException("face embed doesn't have a user id registerd");
        User? user = await userRepository.GetUserById(user_id ?? Guid.Empty);
        if (user == null)
            return new UserNotFound(user_id ?? Guid.Empty);
        string AccessToken = jwtProvider.GenerateAccesss(user);
        string RefreshToken = jwtProvider.GenerateReferesh();
        refreshRepository.Create(new RefreshToken
        {
            Id = Guid.NewGuid(),
            ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
            Token = RefreshToken,
            UserId = user_id ?? Guid.Empty
        });
        await unitOfWork.SaveChangesAsync();
        return new LoginUserCommandResponse(RefreshToken, AccessToken);
    }

}

internal sealed class LoginWithFaceCommandValidator : AbstractValidator<LoginWithFaceCommand>
{
    public LoginWithFaceCommandValidator()
    {
        RuleFor(x => x.imgBinaries).NotEmpty();
    }
}
