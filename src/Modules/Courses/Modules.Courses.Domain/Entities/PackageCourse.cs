namespace Modules.Courses.Domain.Entities;

public class PackageCourse
{
    public Guid id { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid ToutorId { get; private set; }
    public Guid PackageId { get; private set; }
    public Course? Course { get; private set; }
    public Package? Package { get; private set; }
}