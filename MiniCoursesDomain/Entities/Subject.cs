using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;

namespace MiniCoursesDomain.Entities;

public class Subject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Semester Semester { get; set; }
    public User Professor { get; set; }
    public string Code { get; set; }
    public SemesterType SemesterType { get; set; }
    public int Year { get; set; }
}