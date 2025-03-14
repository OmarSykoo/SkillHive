namespace Modules.Users.Domain;
public interface IUserRepository
{
    Task<User> GetUserByUserName(string username);
    Task<User> GetUserByEmail(string Email);
    Task<User> GetUserById(Guid id);
    Task<Guid> CreateUser(User user);
    Task UpdateUser(User user);
}
