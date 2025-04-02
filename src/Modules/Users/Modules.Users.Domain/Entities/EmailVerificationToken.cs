using Modules.Common.Domain.Entities;
using Modules.Users.Domain.DomainEvents;

namespace Modules.Users.Domain.Entities;

public class EmailVerificationToken : Entity
{
    public Guid UserId { get; private set; }
    public virtual UnverifiedUser? User { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime ExpiresOnUtc { get; private set; }
    public static EmailVerificationToken Create(Guid userId, int LifeTimeInMinutes)
    {
        var emailToken = new EmailVerificationToken()
        {
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            CreatedOnUtc = DateTime.Now,
            ExpiresOnUtc = DateTime.Now.AddMinutes(LifeTimeInMinutes)
        };
        emailToken.RaiseDomainEvent(new EmailTokenCreatedDomainEvent(emailToken.Token));
        return emailToken;
    }

}
