using Modules.Users.Domain.Entities;

namespace Modules.Users.Domain.Repositories;

public interface IEmailVerificationTokenRepository
{
    public Task<EmailVerificationToken> GetByToken(string Token);
}
