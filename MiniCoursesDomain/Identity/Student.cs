using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCoursesDomain.Identity
{
    public class Student : IdentityUser
    {
        public string? Indeks { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}
