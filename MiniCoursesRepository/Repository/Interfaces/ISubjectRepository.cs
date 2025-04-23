using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;

namespace MiniCoursesRepository.Repository.Interfaces;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllAsync();
    Task<Subject?> GetByIdAsync(Guid id);
    Task AddAsync(Subject? subject);
    Task UpdateAsync(Subject? subject);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Subject>> GetBySemesterTypeAsync(SemesterType semesterType);
}