using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCoursesRepository;
using MiniCoursesDomain.Entities;
using System.Security.Claims;

namespace MiniCoursesIKTProject.Controllers
{
    public class SubjectPages : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectPages(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Homework(Guid subjectId)
        {
            var subject = await _context.Subjects
                .Include(s => s.Homework)
                .FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

    }
}
