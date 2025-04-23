using MiniCoursesDomain.Identity;

namespace MiniCoursesRepository.Repository.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}