using MiniCoursesDomain.Identity;

namespace MiniCoursesRepository.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task CreateAsync(User user, string password, string role);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}