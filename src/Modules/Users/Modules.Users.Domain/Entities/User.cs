using System.Globalization;
using Modules.Common.Domain.Entities;
using Modules.Common.Domain.Exceptions;

namespace Modules.Users.Domain;

public class User : Entity
{
    private User() { }
    public Guid id { get; private set; } = Guid.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateOnly DateOfCreation { get; private set; } = new DateOnly();
    public Location location { get; private set; } = new Location();
    public Double Balance { get; private set; } = 0.0;
    public static User Create(string UserName, string HashedPassword, string Role, string Email, string PhoneNumber, string state, string city, string locationDesc)
    {

        var user = new User()
        {
            id = Guid.NewGuid()
        ,
            UserName = UserName
        ,
            HashedPassword = HashedPassword
        ,
            Email = Email
        ,
            PhoneNumber = PhoneNumber
        ,
            Balance = 0.0
        ,
            DateOfCreation = DateOnly.FromDateTime(DateTime.UtcNow)
        ,
            location = new Location(state, city, locationDesc)
        ,
            Role = Role
        };
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.id));
        return user;
    }

    public void UpdatePassword(string newHashedPassword)
    {
        if (newHashedPassword == this.HashedPassword)
            return;
        this.HashedPassword = newHashedPassword;
    }

    public void UpdateUserName(string newUserName)
    {
        if (newUserName == this.UserName)
            return;
        this.UserName = newUserName;
    }
}
