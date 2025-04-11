namespace MiniCoursesDomain.Entities;

public class GradedFile
{
    public Guid Id { get; set; }
    public byte[] File { get; set; }
    public double Grade { get; set; }
    public Homework Homework { get; set; }
}