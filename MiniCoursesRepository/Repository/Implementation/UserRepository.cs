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
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.SemesterApplications)
                    .ThenInclude(sa => sa.Subjects)
                        .ThenInclude(ss => ss.Subject)
                .Include(u => u.SubjectsGrades)
                    .ThenInclude(sg => sg.Subject)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _userManager.Users
                .Include(u => u.SemesterApplications)
                    .ThenInclude(sa => sa.Subjects)
                        .ThenInclude(ss => ss.Subject)
                .Include(u => u.SubjectsGrades)
                    .ThenInclude(sg => sg.Subject)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task CreateAsync(User user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task UpdateAsync(User user)
        {
            var existing = await _userManager.Users
                .Include(u => u.SemesterApplications)
                    .ThenInclude(sa => sa.Subjects)
                        .ThenInclude(ss => ss.Subject)
                .Include(u => u.SubjectsGrades)
                    .ThenInclude(sg => sg.Subject)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existing == null)
                throw new Exception("User not found.");

            // Update basic user properties
            existing.Name = user.Name;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.Indeks = user.Indeks;
            existing.UserName = user.UserName;

            // Update collections
            existing.SemesterApplications = user.SemesterApplications;
            existing.SubjectsGrades = user.SubjectsGrades;

            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Ensure changes are saved
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Student");
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}