using MiniCoursesDomain.Entities;

namespace MiniCoursesDomain.DTO.ViewModels;

public class AddHomeworkViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid SubjectId { get; set; }
    public List<Subject> Subjects { get; set; } = [];
}