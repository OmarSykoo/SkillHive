using FluentValidation;
using Modules.Common.Application.Messaging;
using Modules.Common.Domain;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Repositories;

namespace Modules.Users.Application.UseCases;

public record CreateFaceEmbedingCommand(byte[] ImgeBinaries, Guid userID) : ICommand;

public sealed class CreateFaceEmbedingCommandHandler(IFaceModelService faceModelService, IFaceEmbedingRepository faceEmbedingRepository) : ICommandHandler<CreateFaceEmbedingCommand>
{
    public async Task<Result> Handle(CreateFaceEmbedingCommand request, CancellationToken cancellationToken)
    {
        ICollection<float> embeding = await faceModelService.FaceImgToEmbeding(request.ImgeBinaries);
        FaceEmbeding faceEmbeding = FaceEmbeding.Create(request.userID, embeding);
        await faceEmbedingRepository.AddEmbeding(faceEmbeding);
        return Result.Success();
    }
}

internal sealed class CreateFaceEmbedingCommandValidator : AbstractValidator<CreateFaceEmbedingCommand>
{
    public CreateFaceEmbedingCommandValidator()
    {
        RuleFor(x => x.ImgeBinaries).NotEmpty();
        RuleFor(x => x.userID).NotEmpty();
    }
}