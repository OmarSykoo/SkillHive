namespace Modules.Courses.Domain.Entities;

public class MessageContent : CourseContent
{
    public string Message { get; private set; }

    private MessageContent(Guid courseId, string name, string message)
        : base(courseId, name)
    {
        Message = message;
    }

    public static MessageContent Create(Guid courseId, string name, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        return new MessageContent(courseId, name, message);
    }
}
