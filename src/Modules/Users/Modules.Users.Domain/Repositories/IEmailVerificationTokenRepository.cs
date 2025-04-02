using Modules.Users.Domain.Entities;

namespace Modules.Users.Domain.Repositories;

public interface IEmailVerificationTokenRepository
{
    public Task<EmailVerificationToken?> GetByToken(string Token);
    public Task<EmailVerificationToken?> GetByUserId(Guid id);
    public Task Create(EmailVerificationToken emailVerificationToken);
}
