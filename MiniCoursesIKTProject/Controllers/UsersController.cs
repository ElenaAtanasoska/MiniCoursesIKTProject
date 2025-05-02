using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain.Identity;
using MiniCoursesService.Interface;
using MiniCoursesDomain.DTO;
using MiniCoursesService.Implementation;
using Microsoft.AspNetCore.Identity;
using Azure.Identity;

namespace MiniCoursesIKTProject.Controllers
{
    //[Authorize(Roles = "Editor")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IUserService studentService, RoleManager<IdentityRole> roleManager)
        {
            _userService = studentService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string selectedRole = null)
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var usersWithRoles = await _userService.GetUsersByRoleAsync(selectedRole);

            ViewBag.SelectedRole = selectedRole;
            ViewBag.Roles = roles;
            return View(usersWithRoles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var roleNames = _roleManager.Roles.Select(r => r.Name).ToList();

            var roles = roleNames
                .Select(role => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = role,
                    Text = role
                })
                .ToList();

            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
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

        [HttpGet]
        public async Task<IActionResult> Grades(string id)
        {
            var user = await _userService.GetUserWithRolesByIdAsync(id);
            if (user == null)
                return NotFound();

            //TODO - Add service method for listing subjects for student
            return View();
        }

        [HttpGet]
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
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }

}
