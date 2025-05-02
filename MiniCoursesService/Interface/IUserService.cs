using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniCoursesDomain;
using MiniCoursesDomain.Identity;

namespace MiniCoursesService.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task CreateAsync(User user, string password, string role);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);

        Task<Dictionary<User, List<string>>> GetUsersByRoleAsync(string role = null);
        Task<Tuple<User, List<string>>> GetUserWithRolesByIdAsync(string id);

    }
}
