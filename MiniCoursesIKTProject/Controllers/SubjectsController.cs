using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository;
using MiniCoursesRepository.Repository.Interfaces;

namespace MiniCoursesIKTProject.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public SubjectController(
            ISubjectRepository subjectRepository,
            UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _subjectRepository = subjectRepository;
            _userManager = userManager;
            _context = context;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users
                .Include(u => u.SubjectsGrades)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            
            if (user == null)
            {
                return Unauthorized();
            }

            IEnumerable<Subject> subjects;
            if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                var enrolledSubjectIds = user.SubjectsGrades
                    .Select(sg => sg.SubjectId)
                    .ToList();
                subjects = await _subjectRepository.GetAllAsync();
                subjects = subjects.Where(s => enrolledSubjectIds.Contains(s.Id)).ToList();
            }
            else
            {
                subjects = await _subjectRepository.GetAllAsync();
            }

            return View(subjects);
        }

        // GET: Subject/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subject/Create
        public async Task<IActionResult> Create()
        {
            await PopulateProfessorsDropdown();
            await PopulateSemesterDropdowns();
            return View();
        }

        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (string.IsNullOrEmpty(subject.ProfessorId))
            {
                ModelState.AddModelError("ProfessorId", "The Professor field is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subject.Id = Guid.NewGuid();
                    await _subjectRepository.AddAsync(subject);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving subject: " + ex.Message);
                }
            }

            await PopulateProfessorsDropdown(subject.ProfessorId);
            await PopulateSemesterDropdowns();
            return View(subject);
        }

        // GET: Subject/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            await PopulateProfessorsDropdown(subject.ProfessorId);
            await PopulateSemesterDropdowns();
            return View(subject);
        }

        // POST: Subject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Semester,ProfessorId,Code,SemesterType,Year")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _subjectRepository.UpdateAsync(subject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubjectExists(subject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateProfessorsDropdown(subject.ProfessorId);
            await PopulateSemesterDropdowns();
            return View(subject);
        }

        // GET: Subject/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _subjectRepository.GetByIdAsync(id.Value);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _subjectRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubjectExists(Guid id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            return subject != null;
        }

        private async Task PopulateProfessorsDropdown(object selectedProfessor = null)
        {
            var professors = await _userManager.GetUsersInRoleAsync("Professor");
            var professorList = professors.OrderBy(p => p.LastName)
                .Select(p => new 
                { 
                    Id = p.Id, 
                    Name = $"{p.Name} {p.LastName}"
                }).ToList();
    
            ViewBag.ProfessorId = new SelectList(professorList, "Id", "Name", selectedProfessor);
        }

        private Task PopulateSemesterDropdowns()
        {
            ViewBag.SemesterType = new SelectList(Enum.GetValues(typeof(SemesterType)).Cast<SemesterType>());
    
            ViewBag.Semester = new SelectList(Enum.GetValues(typeof(Semester)).Cast<Semester>());
    
            return Task.CompletedTask;
        }
    }
}