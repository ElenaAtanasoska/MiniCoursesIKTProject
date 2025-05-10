using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesRepository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniCoursesRepository.Repository.Implementation;

public class HomeworkRepository : IHomeworkRepository
{
    private readonly ApplicationDbContext _context;

    public HomeworkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Homework>> GetAllAsync()
    {
        return await _context.Homeworks
            .Include(h => h.CreatedBy)
            .Include(h => h.Subject)
            .Include(h => h.Files)
            .ToListAsync();
    }

    public async Task<Homework> GetByIdAsync(Guid id)
    {
        return await _context.Homeworks
            .Include(h => h.CreatedBy)
            .Include(h => h.Subject)
            .Include(h => h.Files)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task AddAsync(Homework homework)
    {
        if (homework == null)
        {
            throw new ArgumentNullException(nameof(homework));
        }
        await _context.Homeworks.AddAsync(homework);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Homework homework)
    {
        if (homework == null)
        {
            throw new ArgumentNullException(nameof(homework));
        }
        _context.Homeworks.Update(homework);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var homework = await _context.Homeworks.FindAsync(id);
        if (homework != null)
        {
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
        }
    }
}