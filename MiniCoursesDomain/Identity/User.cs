using Microsoft.AspNetCore.Identity;
using MiniCoursesDomain.Entities;

namespace MiniCoursesDomain.Identity;

public class User : IdentityUser
{
    public string? Indeks { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public List<StudentSubject> SubjectsGrades { get; set; } = [];
    public List<GradedFile> GradedFiles { get; set; } = [];
    public List<SemesterApplication> SemesterApplications { get; set; } = [];
}