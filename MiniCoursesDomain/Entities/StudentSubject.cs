using MiniCoursesDomain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MiniCoursesDomain.Entities;

public class StudentSubject
{
    public Guid Id { get; set; }

    [Required]
    public Guid SubjectId { get; set; }

    public Subject Subject { get; set; }

    public int? Grade { get; set; }

    public SubjectRequestStatus RequestStatus { get; set; } = SubjectRequestStatus.Pending;
}