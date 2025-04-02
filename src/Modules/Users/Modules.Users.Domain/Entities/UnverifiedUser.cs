using System.Security.Cryptography.X509Certificates;
using Modules.Common.Domain.Entities;
using Modules.Users.Domain.DomainEvents;

namespace Modules.Users.Domain.Entities;

public class UnverifiedUser : Entity
{
    public Guid id { get; set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime DateOfCreation { get; private set; }
    public Location location { get; private set; } = new Location();
    public double Balance { get; private set; } = 0.0;
    public static UnverifiedUser Create(string FirstName, string LastName, string HashedPassword, string Role, string Email, string PhoneNumber, string state, string city, string locationDesc)
    {
        var user = new UnverifiedUser()
        {
            id = Guid.NewGuid(),
            FirstName = FirstName,
            LastName = LastName,
            HashedPassword = HashedPassword,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Balance = 0.0,
            DateOfCreation = DateTime.Now,
            location = new Location(state, city, locationDesc),
            Role = Role
        };
        user.RaiseDomainEvent(new UnverifiedUserCreatedDomainEvent(user.id));
        return user;
    }
}
