using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace Modules.Courses.Domain.Entities;

public class Customer
{
    public Guid id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
}

public class Tutor
{
    public Guid id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
}

public class Course
{
    public Guid id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImgUrl { get; private set; } = string.Empty;
    public double Price { get; private set; } = 0.0;
    public Guid? TutorId { get; private set; }
    public Tutor? tutor { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public static Course Create(string Title, string Description, string ImgUrl, double Price, Guid TutorId)
    {
        return new Course
        {
            id = Guid.NewGuid(),
            Title = Title,
            Description = Description,
            ImgUrl = ImgUrl,
            Price = Price,
            TutorId = TutorId,
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}

public class Package
{
    public Guid id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImgUrl { get; private set; } = string.Empty;
    public double Price { get; private set; }
    public Guid? TutorId { get; private set; }
    public Tutor? Tutor { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public static Package Create(string Title, string Description, string ImgUrl, double Price, Guid TutorId)
    {
        return new Package
        {
            id = Guid.NewGuid(),
            Title = Title,
            Description = Description,
            ImgUrl = ImgUrl,
            Price = Price,
            TutorId = TutorId,
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}

public class PackageCourse
{
    public Guid id { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid PackageId { get; private set; }
    public Course? Course { get; private set; }
    public Package? Package { get; private set; }
}