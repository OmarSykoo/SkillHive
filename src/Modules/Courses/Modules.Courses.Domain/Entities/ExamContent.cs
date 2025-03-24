namespace Modules.Courses.Domain.Entities;

public class ExamContent : CourseContent
{
    public double PassingPercentage { get; private set; }

    private ExamContent(Guid courseId, string name, double passingPercentage)
        : base(courseId, name)
    {
        if (passingPercentage < 0 || passingPercentage > 100)
            throw new ArgumentException("Passing percentage must be between 0 and 100.");

        PassingPercentage = passingPercentage;
    }

    public static ExamContent Create(Guid courseId, string name, double passingPercentage)
    {
        return new ExamContent(courseId, name, passingPercentage);
    }
}
