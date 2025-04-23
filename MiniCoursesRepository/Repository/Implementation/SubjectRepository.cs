using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesRepository;
using MiniCoursesRepository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniCoursesRepository.Repository.Implementation
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                .ToListAsync();
        }

        public async Task<Subject> GetByIdAsync(Guid id)
        {
            return await _context.Subjects
                .Include(s => s.Professor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subject subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subject>> GetBySemesterTypeAsync(SemesterType semesterType)
        {
            return await _context.Subjects
                .Where(s => s.SemesterType == semesterType)
                .ToListAsync();
        }
    }
}