using System.Globalization;
using System.Net.Http.Headers;
using Modules.Common.Domain.Entities;
using Modules.Common.Domain.Exceptions;

namespace Modules.Users.Domain;

public class User : Entity
{
    private User() { }
    public Guid id { get; private set; } = Guid.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime DateOfCreation { get; private set; }
    public Location location { get; private set; } = new Location();
    public Double Balance { get; private set; } = 0.0;
    public static User Create(string FirstName, string LastName, string HashedPassword, string Role, string Email, string PhoneNumber, string state, string city, string locationDesc)
    {

        var user = new User()
        {
            id = Guid.NewGuid()
        ,
            FirstName = FirstName
        ,
            LastName = LastName
        ,
            HashedPassword = HashedPassword
        ,
            Email = Email
        ,
            PhoneNumber = PhoneNumber
        ,
            Balance = 0.0
        ,
            DateOfCreation = DateTime.Now
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

    public void UpdateName(string FirstName, string LastName)
    {
        if (FirstName == this.FirstName && LastName == this.LastName)
        {
            return;
        }
        if (FirstName is not null)
            this.FirstName = FirstName;
        if (LastName is not null)
            this.LastName = LastName;
        this.RaiseDomainEvent(new UserFirstLastNameUpdated(this.id));
    }
}
