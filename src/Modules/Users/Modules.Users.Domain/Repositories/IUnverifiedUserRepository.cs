using Modules.Users.Domain.Entities;

namespace Modules.Users.Domain.Repositories;

public interface IUnverifiedUserRepository
{
    public Task<UnverifiedUser?> GetById(Guid id);
    public void Create(UnverifiedUser UnverifiedUser);
}
