namespace Modules.Courses.Domain.Entities;

public class VideoContent : CourseContent
{
    public string VideoUrl { get; private set; }

    private VideoContent(Guid courseId, string name, string videoUrl)
        : base(courseId, name)
    {
        VideoUrl = videoUrl;
    }

    public static VideoContent Create(Guid courseId, string name, string videoUrl)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
            throw new ArgumentException("Video URL cannot be empty.");

        return new VideoContent(courseId, name, videoUrl);
    }
}
