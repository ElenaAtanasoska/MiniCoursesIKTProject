using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCoursesRepository.Repository.Implementation
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<User> _userManager;

        public StudentRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private async Task<bool> IsStudent(User user)
        {
            return await _userManager.IsInRoleAsync(user, "Student");
        }


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<User>();

            foreach (var user in users)
            {
                if (await IsStudent(user))
                    result.Add(user);
            }

            return result;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && await IsStudent(user))
                return user;

            return null;
        }

        public async Task CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Student");
        }

        public async Task UpdateAsync(User user)
        {
            var existing = await _userManager.FindByIdAsync(user.Id);
            if (existing == null || !(await IsStudent(existing)))
                throw new Exception("User not found or not a student.");

            existing.Name = user.Name;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.Indeks = user.Indeks;
            existing.UserName = user.UserName;

            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !(await IsStudent(user)))
                throw new Exception("User not found or not a student.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

}
