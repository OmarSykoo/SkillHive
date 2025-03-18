using Modules.Users.Domain.Entities;

namespace Modules.Users.Domain.Repositories;

public interface IFaceEmbedingRepository
{
    public Task AddEmbeding(FaceEmbeding faceEmbeding);
    public Task<(float? score, Guid? userId)> GetMatching(ICollection<float> embeding);
}
