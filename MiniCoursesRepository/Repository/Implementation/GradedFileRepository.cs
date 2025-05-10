using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesRepository.Repository.Interfaces;

namespace MiniCoursesRepository.Repository.Implementation;

public class GradedFileRepository : IGradedFileRepository
{
    private readonly ApplicationDbContext _context;

    public GradedFileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GradedFile>> GetAllAsync()
    {
        return await _context.GradedFiles
            .Include(gf => gf.Homework)
            .OrderByDescending(gf => gf.DateUploaded)
            .ToListAsync();
    }

    public async Task<GradedFile> GetByIdAsync(Guid id)
    {
        return await _context.GradedFiles
            .Include(gf => gf.Homework)
            .FirstOrDefaultAsync(gf => gf.Id == id);
    }

    public async Task AddAsync(GradedFile gradedFile)
    {
        if (gradedFile == null)
        {
            throw new ArgumentNullException(nameof(gradedFile));
        }
        await _context.GradedFiles.AddAsync(gradedFile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(GradedFile gradedFile)
    {
        if (gradedFile == null)
        {
            throw new ArgumentNullException(nameof(gradedFile));
        }
        _context.GradedFiles.Update(gradedFile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var gradedFile = await _context.GradedFiles.FindAsync(id);
        if (gradedFile != null)
        {
            _context.GradedFiles.Remove(gradedFile);
            await _context.SaveChangesAsync();
        }
    }
}