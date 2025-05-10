using MiniCoursesDomain.Entities;

namespace MiniCoursesRepository.Repository.Interfaces;

public interface IHomeworkRepository
{
    Task<IEnumerable<Homework>> GetAllAsync();
    Task<Homework> GetByIdAsync(Guid id);
    Task AddAsync(Homework homework);
    Task UpdateAsync(Homework homework);
    Task DeleteAsync(Guid id);
}