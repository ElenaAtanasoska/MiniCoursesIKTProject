using MiniCoursesDomain.Entities;

namespace MiniCoursesRepository.Repository.Interfaces;

public interface IGradedFileRepository
{
    Task<IEnumerable<GradedFile>> GetAllAsync();
    Task<GradedFile> GetByIdAsync(Guid id);
    Task AddAsync(GradedFile gradedFile);
    Task UpdateAsync(GradedFile gradedFile);
    Task DeleteAsync(Guid id);
}