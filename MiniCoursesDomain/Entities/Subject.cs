        using System.ComponentModel.DataAnnotations;
        using MiniCoursesDomain.Enums;
        using MiniCoursesDomain.Identity;

        namespace MiniCoursesDomain.Entities;

        public class Subject
        {
            public Guid Id { get; set; }

            [Required]
            public string Name { get; set; }

            public Semester Semester { get; set; }

            [Required]
            public string ProfessorId { get; set; }

            public User? Professor { get; set; }

            [Required]
            public string Code { get; set; }

            public SemesterType SemesterType { get; set; }

            [Range(2000, 2100)]
            public int Year { get; set; }
        }