using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository;

namespace MiniCoursesIKTProject.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Index1(string? searchQuery)
    {
        var user = await _userManager.GetUserAsync(User);

        var applications = await _context.SemesterApplications
            .Where(sa => sa.StudentId == user.Id && sa.Status == SubjectRequestStatus.Accepted)
            .Include(sa => sa.Subjects)
                .ThenInclude(ss => ss.Subject)
            .ToListAsync();

        var subjects = applications
            .SelectMany(sa => sa.Subjects)
            .Where(ss => ss.Subject != null)
            .ToList();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            subjects = subjects
                .Where(ss => ss.Subject.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return View(subjects);
    }
    public IActionResult Users()
    {
        var users = new List<User>
        {
            new User
            {
                UserName = "alice123",
                Name = "Alice",
                LastName = "Smith",
                Indeks = "201001"
            },
            new User
            {
                UserName = "bob456",
                Name = "Bob",
                LastName = "Johnson",
                Indeks = "201002"
            }
        };

        return View(users);
    }

    public IActionResult ProfilePage()
    {
        var user = new User
        {
            UserName = "john.doe",
            Name = "John",
            LastName = "Doe",
            Indeks = "201045",
            SubjectsGrades = new List<StudentSubject>
            {
                new StudentSubject
                {
                    Subject = new Subject { Name = "Mathematics" },
                    Grade = 7,
                    RequestStatus = SubjectRequestStatus.Pending
                },
                new StudentSubject
                {
                    Subject = new Subject { Name = "Programming" },
                    Grade = 10,
                    RequestStatus = SubjectRequestStatus.Pending
                },
                new StudentSubject
                {
                    Subject = new Subject { Name = "Algorithms" },
                    Grade = null,
                    RequestStatus = SubjectRequestStatus.Pending
                },
                new StudentSubject
                {
                    Subject = new Subject { Name = "Databases" },
                    Grade = 8,
                    RequestStatus = SubjectRequestStatus.Pending
                }
            }
        };

        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}