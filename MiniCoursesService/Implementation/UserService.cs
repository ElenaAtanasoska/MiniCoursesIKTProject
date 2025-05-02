using Microsoft.AspNetCore.Identity;
using MiniCoursesDomain.Identity;
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

        public UserService(IUserRepository studentRepository, UserManager<User> userManager)
        {
            _userRepository = studentRepository;
            _userManager = userManager;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _userRepository.GetAllAsync();

        public Task<User> GetByIdAsync(string id) => _userRepository.GetByIdAsync(id);

        public Task CreateAsync(User student, string password, string role) =>
            _userRepository.CreateAsync(student, password, role);

        public Task UpdateAsync(User student) => _userRepository.UpdateAsync(student);

        public Task DeleteAsync(string id) => _userRepository.DeleteAsync(id);

        public async Task<Dictionary<User, List<string>>> GetUsersByRoleAsync(string role = null)
        {
            var users = await _userRepository.GetAllAsync();
            var userRoleDictionary = new Dictionary<User, List<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!string.IsNullOrEmpty(role) && !roles.Contains(role))
                {
                    continue;
                }

                userRoleDictionary[user] = roles.ToList();
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
