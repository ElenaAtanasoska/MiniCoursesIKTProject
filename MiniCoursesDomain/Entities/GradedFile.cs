using MiniCoursesDomain.Identity;

namespace MiniCoursesDomain.Entities;

public class GradedFile
{
    public Guid Id { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public int? Grade { get; set; }
    public Guid HomeworkId { get; set; }
    public Homework Homework { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime DateUploaded { get; set; } = DateTime.UtcNow;
}