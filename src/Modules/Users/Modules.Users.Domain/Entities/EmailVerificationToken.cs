namespace Modules.Users.Domain.Entities;

public class EmailVerificationToken
{
    public Guid UserId { get; private set; }
    public virtual User? User { get; private set; }
    public string Token { get; private set; } = string.Empty;
}
