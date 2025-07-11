namespace Modules.Courses.Domain.Entities;

public class Customer
{
    public Guid id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
}
