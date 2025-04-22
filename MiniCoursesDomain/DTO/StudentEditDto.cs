using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCoursesDomain.DTO
{
    public class StudentEditDto
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Indeks { get; set; }
    }
}
