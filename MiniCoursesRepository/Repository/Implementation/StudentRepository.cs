﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Interfaces;

namespace MiniCoursesRepository.Repository.Implementation
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public StudentRepository(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private async Task<bool> IsStudent(User user)
        {
            return await _userManager.IsInRoleAsync(user, "Student");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users
                .Include(u => u.SemesterApplications)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.SemesterApplications)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var existing = await _userManager.Users
                .Include(u => u.SemesterApplications)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existing == null)
                throw new Exception("User not found.");

            existing.Name = user.Name;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.Indeks = user.Indeks;
            existing.UserName = user.UserName;

            existing.SemesterApplications = user.SemesterApplications;

            var result = await _userManager.UpdateAsync(existing);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

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
            if (user == null || !(await IsStudent(user)))
                throw new Exception("User not found or not a student.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}