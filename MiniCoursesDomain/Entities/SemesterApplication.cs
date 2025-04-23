using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniCoursesDomain.Entities;

public class SemesterApplication
{
    public Guid Id { get; set; }

    [Required]
    public Semester Semester { get; set; }

    [Required]
    public SemesterType SemesterType { get; set; }

    [Range(2000, 2100)]
    public int Year { get; set; }

    public SubjectRequestStatus Status { get; set; } = SubjectRequestStatus.Pending;

    [Required]
    public string StudentId { get; set; }

    public User? Student { get; set; }

    [Required(ErrorMessage = "You must select 3 to 6 subjects.")]
    [MinLength(3, ErrorMessage = "You must select at least 3 subjects.")]
    [MaxLength(6, ErrorMessage = "You cannot select more than 6 subjects.")]
    public List<StudentSubject> Subjects { get; set; } = new List<StudentSubject>();
}

public class MinLengthAttribute : ValidationAttribute
{
    private readonly int _minLength;

    public MinLengthAttribute(int minLength)
    {
        _minLength = minLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IList<StudentSubject> list && list.Count < _minLength)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}

public class MaxLengthAttribute : ValidationAttribute
{
    private readonly int _maxLength;

    public MaxLengthAttribute(int maxLength)
    {
        _maxLength = maxLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IList<StudentSubject> list && list.Count > _maxLength)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}