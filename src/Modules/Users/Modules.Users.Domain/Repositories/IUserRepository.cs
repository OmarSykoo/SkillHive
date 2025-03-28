namespace Modules.Users.Domain;
public interface IUserRepository
{
    Task<User?> GetUserByEmail(string Email, bool verified = true);
    Task<User?> GetUserById(Guid id, bool verified = true);
    Task<Guid> CreateUser(User user);
    Task UpdateUser(User user);
}
