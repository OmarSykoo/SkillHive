namespace Modules.Courses.Domain.Entities;

public abstract class CourseContent
{
    public Guid Id { get; private set; }
    public Guid CourseId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    protected CourseContent(Guid courseId, string name)
    {
        Id = Guid.NewGuid();
        CourseId = courseId;
        Name = name;
    }
}
