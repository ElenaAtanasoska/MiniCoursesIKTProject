using Microsoft.AspNetCore.Identity;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Implementation;
using MiniCoursesRepository.Repository.Interfaces;
using MiniCoursesService.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCoursesService.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ISubjectRepository _subjectRepository;

        public UserService(IUserRepository studentRepository, UserManager<User> userManager, ISubjectRepository subjectRepository)
        {
            _userRepository = studentRepository;
            _userManager = userManager;
            _subjectRepository = subjectRepository;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _userRepository.GetAllAsync();

        public Task<User> GetByIdAsync(string id) => _userRepository.GetByIdAsync(id);

        public Task CreateAsync(User student, string password, string role) =>
            _userRepository.CreateAsync(student, password, role);

        public Task UpdateAsync(User student) => _userRepository.UpdateAsync(student);

        public Task DeleteAsync(string id) => _userRepository.DeleteAsync(id);

        public async Task<Dictionary<User, List<string>>> FilterUsersByRoleAndSubject(List<string> roles = null, Guid? subjectId = null)
        {
            var users = await _userRepository.GetAllAsync();
            var userRoleDictionary = new Dictionary<User, List<string>>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (roles != null && roles.Any())
                {
                    if (!userRoles.Any(r => roles.Contains(r)))
                    {
                        continue;
                    }
                }

                if (subjectId.HasValue)
                {
                    var subject = _subjectRepository.GetByIdAsync(subjectId.Value);

                    var matchesAsStudent = user.SubjectsGrades.Any(sg => sg.SubjectId == subjectId);
                    var matchesAsProfessor = subject.Result.ProfessorId == user.Id;

                    if (!matchesAsStudent && !matchesAsProfessor)
                    {
                        continue;
                    }
                }

                userRoleDictionary[user] = userRoles.ToList();
            }

            return userRoleDictionary;
        }

        public async Task<Dictionary<User, List<string>>> GetUsersByRolesAsync(List<string> roles = null)
        {
            var users = await _userRepository.GetAllAsync();
            var userRoleDictionary = new Dictionary<User, List<string>>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (roles != null && roles.Any())
                {
                    if (!userRoles.Any(r => roles.Contains(r)))
                    {
                        continue;
                    }
                }

                userRoleDictionary[user] = userRoles.ToList();
            }

            return userRoleDictionary;
        }


        public async Task<Tuple<User, List<string>>> GetUserWithRolesByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Tuple.Create(user, roles.ToList());
        }
    }

}
