using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain.Identity;
using MiniCoursesService.Interface;
using MiniCoursesDomain.DTO;

namespace MiniCoursesIKTProject.Controllers
{
    //[Authorize(Roles = "Editor")]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        { 
            var students = await _studentService.GetAllStudentsAsync();
            return View(students.ToList()); 
        }

        public async Task<IActionResult> Details(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateDto dto)
        {
            var student = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                Indeks = dto.Indeks
            };

            await _studentService.CreateStudentAsync(student, dto.Password);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            var dto = new StudentEditDto
            {
                Id = student.Id,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Indeks = student.Indeks
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentEditDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var updatedStudent = new User
            {
                Id = dto.Id,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Indeks = dto.Indeks
            };

            await _studentService.UpdateStudentAsync(updatedStudent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }
    }

}
