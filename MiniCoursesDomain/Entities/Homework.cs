using MiniCoursesDomain.Identity;

namespace MiniCoursesDomain.Entities;

public class Homework
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<GradedFile> Files { get; set; } = [];
    public string CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
}