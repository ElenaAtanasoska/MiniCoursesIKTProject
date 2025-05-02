using Microsoft.AspNetCore.Mvc;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Enums;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiniCourses.Controllers
{
    public class SemesterController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;

        public SemesterController(IUserRepository userRepository, ISubjectRepository subjectRepository)
        {
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
        }

        // GET: Semester/Apply
        [HttpGet]
        public IActionResult Apply()
        {
            var model = new SemesterApplicationViewModel
            {
                Year = DateTime.Now.Year,
                AvailableSubjects = new List<SelectListItem>()
            };
            return View(model);
        }

        // POST: Semester/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(SemesterApplicationViewModel model)
        {
            if (model.SelectedSubjectIds == null || model.SelectedSubjectIds.Count < 3 || model.SelectedSubjectIds.Count > 6)
            {
                ModelState.AddModelError("SelectedSubjectIds", "You must select 3 to 6 subjects.");
            }

            if (!ModelState.IsValid)
            {
                var subjects = await _subjectRepository.GetBySemesterTypeAsync(model.SemesterType);
                model.AvailableSubjects = subjects.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Name} ({s.Code})"
                }).ToList();
                return View(model);
            }

            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentId))
            {
                ModelState.AddModelError("", "User is not logged in.");
                return View(model);
            }

            var application = new SemesterApplication
            {
                Semester = model.Semester,
                SemesterType = model.SemesterType,
                Year = model.Year,
                StudentId = studentId,
                Status = SubjectRequestStatus.Pending,
                Subjects = model.SelectedSubjectIds.Select(id => new StudentSubject
                {
                    SubjectId = Guid.Parse(id),
                    RequestStatus = SubjectRequestStatus.Pending
                }).ToList()
            };

            var user = await _userRepository.GetByIdAsync(studentId);
            if (user == null)
            {
                ModelState.AddModelError("", "Student not found.");
                return View(model);
            }

            user.SemesterApplications.Add(application);
            await _userRepository.UpdateAsync(user);

            return RedirectToAction("ApplicationStatus");
        }

        // GET: Semester/GetSubjects
        [HttpGet]
        public async Task<IActionResult> GetSubjects(SemesterType semesterType)
        {
            var subjects = await _subjectRepository.GetBySemesterTypeAsync(semesterType);
            var result = subjects.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                code = s.Code
            });
            return Json(result);
        }

        // GET: Semester/ApplicationStatus
        [HttpGet]
        public async Task<IActionResult> ApplicationStatus()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentId))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userRepository.GetByIdAsync(studentId);
            if (user == null)
            {
                return NotFound();
            }

            var applications = user.SemesterApplications;
            return View(applications);
        }

        // GET: Semester/ManageApplications
        [HttpGet]
        public async Task<IActionResult> ManageApplications()
        {
            var users = await _userRepository.GetAllAsync();
            var pendingApplications = users
                .SelectMany(u => u.SemesterApplications)
                .Where(a => a.Status == SubjectRequestStatus.Pending)
                .ToList();
            return View(pendingApplications);
        }

        // POST: Semester/AcceptApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptApplication(Guid id)
        {
            var users = await _userRepository.GetAllAsync();
            var application = users
                .SelectMany(u => u.SemesterApplications)
                .FirstOrDefault(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = SubjectRequestStatus.Accepted;
            foreach (var subject in application.Subjects)
            {
                subject.RequestStatus = SubjectRequestStatus.Accepted;
            }

            var user = users.FirstOrDefault(u => u.SemesterApplications.Contains(application));
            if (user != null)
            {
                await _userRepository.UpdateAsync(user);
            }

            return RedirectToAction("ManageApplications");
        }

        // POST: Semester/RejectApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectApplication(Guid id)
        {
            var users = await _userRepository.GetAllAsync();
            var application = users
                .SelectMany(u => u.SemesterApplications)
                .FirstOrDefault(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = SubjectRequestStatus.Rejected;
            foreach (var subject in application.Subjects)
            {
                subject.RequestStatus = SubjectRequestStatus.Rejected;
            }

            var user = users.FirstOrDefault(u => u.SemesterApplications.Contains(application));
            if (user != null)
            {
                await _userRepository.UpdateAsync(user);
            }

            return RedirectToAction("ManageApplications");
        }
    }

    public class SemesterApplicationViewModel
    {
        public Semester Semester { get; set; }
        public SemesterType SemesterType { get; set; }
        public int Year { get; set; }
        public List<string> SelectedSubjectIds { get; set; } = [];
        public List<SelectListItem> AvailableSubjects { get; set; } = [];
    }
}