namespace Modules.Users.Domain;
public interface IUserRepository
{
    Task<User?> GetUserByEmail(string Email);
    Task<User?> GetUserById(Guid id);
    Guid CreateUser(User user);
    Task UpdateUser(User user);
}
