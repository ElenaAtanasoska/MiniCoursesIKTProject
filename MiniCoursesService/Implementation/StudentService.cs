using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Interfaces;
using MiniCoursesService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCoursesService.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Task<IEnumerable<User>> GetAllStudentsAsync() => _studentRepository.GetAllAsync();

        public Task<User> GetStudentByIdAsync(string id) => _studentRepository.GetByIdAsync(id);

        public Task CreateStudentAsync(User student, string password) =>
            _studentRepository.CreateAsync(student, password);

        public Task UpdateStudentAsync(User student) => _studentRepository.UpdateAsync(student);

        public Task DeleteStudentAsync(string id) => _studentRepository.DeleteAsync(id);
    }

}
