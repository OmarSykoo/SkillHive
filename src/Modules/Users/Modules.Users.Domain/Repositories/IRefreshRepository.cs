using Modules.Users.Domain.Entities;

namespace Modules.Users.Domain;

public interface IRefreshRepository
{
    void Create(RefreshToken refreshToken);
}
