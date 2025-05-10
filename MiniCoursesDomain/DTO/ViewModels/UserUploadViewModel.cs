namespace MiniCoursesDomain.DTO.ViewModels;

public class UserUploadViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public bool HasUploaded { get; set; }
    public bool IsEnrolled { get; set; }
    public Guid? GradedFileId { get; set; }
    public int? Grade { get; set; }
}