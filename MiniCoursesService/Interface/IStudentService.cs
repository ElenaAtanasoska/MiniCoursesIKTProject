using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCoursesDomain;
using MiniCoursesDomain.Identity;

namespace MiniCoursesService.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<User>> GetAllStudentsAsync();
        Task<User> GetStudentByIdAsync(string id);
        Task CreateStudentAsync(User student, string password);
        Task UpdateStudentAsync(User student);
        Task DeleteStudentAsync(string id);
    }
}
