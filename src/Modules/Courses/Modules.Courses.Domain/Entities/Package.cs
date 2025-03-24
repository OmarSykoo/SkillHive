namespace Modules.Courses.Domain.Entities;

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
