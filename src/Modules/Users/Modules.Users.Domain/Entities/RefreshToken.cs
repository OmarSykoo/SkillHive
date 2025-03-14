namespace Modules.Users.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public virtual User? User { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
}
