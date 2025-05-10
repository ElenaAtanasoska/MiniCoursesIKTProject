namespace MiniCoursesDomain.DTO.ViewModels;

public class HomeworkDetailsViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string SubjectName { get; set; }
    public string CreatedByName { get; set; }
    public List<UserUploadViewModel> UserUploads { get; set; } = [];
}