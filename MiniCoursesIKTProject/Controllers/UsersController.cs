using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain.Identity;
using MiniCoursesService.Interface;
using MiniCoursesDomain.DTO;
using MiniCoursesService.Implementation;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;
using MiniCoursesRepository.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace MiniCoursesIKTProject.Controllers
{
    //[Authorize(Roles = "Editor")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISubjectRepository _subjectRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserService studentService, RoleManager<IdentityRole> roleManager, 
            ISubjectRepository subjectRepository, UserManager<User> userManager)
        {
            _userService = studentService;
            _roleManager = roleManager;
            _subjectRepository = subjectRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string selectedRole = "Student,Professor", Guid? subjectId = null)
        {
            var roles = new List<string> { "Student", "Professor" };

            List<string> selectedRoles = string.IsNullOrEmpty(selectedRole)
                ? roles
                : selectedRole.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToList();

            var usersWithRoles = await _userService.FilterUsersByRoleAndSubject(selectedRoles, subjectId);

            var subjects = await _subjectRepository.GetAllAsync();

            ViewBag.SelectedRole = selectedRole;
            ViewBag.Roles = roles;
            ViewBag.SubjectId = subjectId;
            ViewBag.Subjects = subjects;
            return View(usersWithRoles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            var roles = GetRolesSelectList();

            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with this email already exists.");
                ViewBag.Roles = GetRolesSelectList();
                return View(dto);
            }

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                LastName = dto.LastName,
                Indeks = dto.Role == "Student" ? dto.Indeks : null
            };

            await _userService.CreateAsync(user, dto.Password, dto.Role);
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetRolesSelectList()
        {
            var roleNames = _roleManager.Roles
                .Where(r => r.Name == "Editor" || r.Name == "Professor" || r.Name == "Student")
                .Select(r => r.Name)
                .ToList();

            return roleNames.Select(role => new SelectListItem
            {
                Value = role,
                Text = role
            }).ToList();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetUserWithRolesByIdAsync(id);
            if (user == null)
                return NotFound();

            var dto = new UserEditDto
            {
                Id = user.Item1.Id,
                Name = user.Item1.Name,
                LastName = user.Item1.LastName,
                Email = user.Item1.Email,
                Role = user.Item2.ToList().First()
            };

            if (dto.Role == "Student")
            {
                dto.Indeks = user.Item1.Indeks;
            }

            return View(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(UserEditDto dto)
        {
            Console.WriteLine("Here");
            if (!ModelState.IsValid)
                return View(dto);

            var updatedUser = new User
            {
                Id = dto.Id,
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Indeks = dto.Role == "Student" ? dto.Indeks : null
            };

            await _userService.UpdateAsync(updatedUser);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }

}
