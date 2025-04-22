using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;

namespace MiniCoursesIKTProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
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