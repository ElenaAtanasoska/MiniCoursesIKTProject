using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCoursesDomain.DTO.ViewModels;
using MiniCoursesDomain.Entities;
using MiniCoursesDomain.Identity;
using MiniCoursesRepository.Repository.Interfaces;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using MiniCoursesDomain.Enums;
using MiniCoursesRepository;

namespace MiniCoursesIKTProject.Controllers;

public class HomeworkController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IHomeworkRepository _homeworkRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly IGradedFileRepository _gradedFileRepository;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;

    public HomeworkController(
        UserManager<User> userManager,
        IHomeworkRepository homeworkRepository,
        ISubjectRepository subjectRepository,
        IGradedFileRepository gradedFileRepository,
        IUserRepository userRepository,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _homeworkRepository = homeworkRepository;
        _subjectRepository = subjectRepository;
        _gradedFileRepository = gradedFileRepository;
        _userRepository = userRepository;
        _context = context;
    }

    // GET: Homework/Add
    [HttpGet]
    public async Task<IActionResult> Add(Guid? subjectId)
    {
        var subjects = await _subjectRepository.GetAllAsync();
        var model = new AddHomeworkViewModel
        {
            Subjects = subjects.ToList(),
            SubjectId = subjectId ?? Guid.Empty
        };
        return View(model);
    }

    // POST: Homework/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddHomeworkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Subjects = (await _subjectRepository.GetAllAsync()).ToList();
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var homework = new Homework
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            SubjectId = model.SubjectId,
            CreatedById = user.Id,
            CreatedBy = user
        };

        await _homeworkRepository.AddAsync(homework);
        return RedirectToAction(nameof(List), new { subjectId = model.SubjectId });
    }

    // GET: Homework/List
    [HttpGet]
    public async Task<IActionResult> List(Guid? subjectId)
    {
        var homeworks = await _homeworkRepository.GetAllAsync();
        if (subjectId.HasValue)
        {
            homeworks = homeworks.Where(h => h.SubjectId == subjectId.Value);
        }

        var model = homeworks.Select(h => new HomeworkListViewModel
        {
            Id = h.Id,
            Title = h.Title,
            SubjectName = h.Subject?.Name,
            CreatedByName = $"{h.CreatedBy?.Name} {h.CreatedBy?.LastName}"
        }).ToList();

        ViewBag.SubjectId = subjectId;
        return View(model);
    }

    // GET: Homework/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var homework = await _homeworkRepository.GetByIdAsync(id);
        if (homework == null)
        {
            return NotFound();
        }

        // Get all users
        var users = await _userRepository.GetAllAsync();
        var userUploads = users.Select(u =>
        {
            var gradedFile = u.GradedFiles.FirstOrDefault(gf => gf.HomeworkId == id);
            return new UserUploadViewModel
            {
                UserId = u.Id,
                UserName = $"{u.Name} {u.LastName} ({u.Indeks})",
                HasUploaded = gradedFile != null,
                IsEnrolled = u.SubjectsGrades.Any(sg => sg.SubjectId == homework.SubjectId && sg.RequestStatus == SubjectRequestStatus.Accepted),
                GradedFileId = gradedFile?.Id,
                Grade = gradedFile?.Grade
            };
        }).ToList();

        var model = new HomeworkDetailsViewModel
        {
            Id = homework.Id,
            Title = homework.Title,
            Description = homework.Description,
            SubjectName = homework.Subject?.Name,
            CreatedByName = $"{homework.CreatedBy?.Name} {homework.CreatedBy?.LastName}",
            UserUploads = userUploads
        };

        return View(model);
    }

    // POST: Homework/UploadFile/{homeworkId}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadFile(Guid homeworkId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Please select a file to upload.";
            return RedirectToAction(nameof(Details), new { id = homeworkId });
        }

        var homework = await _homeworkRepository.GetByIdAsync(homeworkId);
        if (homework == null)
        {
            return NotFound();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        if (user.GradedFiles.Any(gf => gf.HomeworkId == homeworkId))
        {
            TempData["Error"] = "You have already uploaded a file for this homework.";
            return RedirectToAction(nameof(Details), new { id = homeworkId });
        }

        if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Only PDF files are allowed.";
            return RedirectToAction(nameof(Details), new { id = homeworkId });
        }

        string pdfText;
        try
        {
            using (var stream = file.OpenReadStream())
            using (var pdfReader = new PdfReader(stream))
            using (var pdfDocument = new PdfDocument(pdfReader))
            {
                var textBuilder = new StringBuilder();
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    var page = pdfDocument.GetPage(i);
                    var text = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
                    textBuilder.Append(text);
                }
                pdfText = textBuilder.ToString();
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Failed to read PDF content: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id = homeworkId });
        }
        
        Console.WriteLine(pdfText); //TODO: call the AI API here

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsDir))
        {
            Directory.CreateDirectory(uploadsDir);
        }

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var gradedFile = new GradedFile
        {
            Id = Guid.NewGuid(),
            FilePath = $"/uploads/{fileName}",
            FileName = file.FileName,
            Grade = null,
            HomeworkId = homeworkId,
            Homework = homework,
            UserId = user.Id,
            User = user
        };

        await _gradedFileRepository.AddAsync(gradedFile);
        await _context.SaveChangesAsync();

        TempData["Success"] = "File uploaded successfully.";
        return RedirectToAction(nameof(Details), new { id = homeworkId });
    }

    // GET: Homework/DownloadFile/{fileId}
    [HttpGet]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        var gradedFile = await _gradedFileRepository.GetByIdAsync(fileId);
        if (gradedFile == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", gradedFile.FilePath.TrimStart('/'));
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(stream, "application/octet-stream", gradedFile.FileName);
    }

    // POST: Homework/GradeFile/{fileId}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GradeFile(Guid fileId, int grade)
    {
        if (grade < 0 || grade > 100)
        {
            TempData["Error"] = "Grade must be between 0 and 100.";
            return RedirectToAction(nameof(Details));
        }

        var gradedFile = await _gradedFileRepository.GetByIdAsync(fileId);
        if (gradedFile == null)
        {
            return NotFound();
        }

        gradedFile.Grade = grade;
        await _gradedFileRepository.UpdateAsync(gradedFile);

        var user = await _userRepository.GetByIdAsync(gradedFile.UserId);
        var homework = await _homeworkRepository.GetByIdAsync(gradedFile.HomeworkId);
        if (user != null && homework != null)
        {
            var subjectId = homework.SubjectId;
            var studentSubject = user.SubjectsGrades.FirstOrDefault(sg => sg.SubjectId == subjectId && sg.RequestStatus == SubjectRequestStatus.Accepted);
            if (studentSubject != null)
            {
                var subjectHomeworks = await _homeworkRepository.GetAllAsync();
                subjectHomeworks = subjectHomeworks.Where(h => h.SubjectId == subjectId);
                var gradedFiles = user.GradedFiles
                    .Where(gf => subjectHomeworks.Any(h => h.Id == gf.HomeworkId) && gf.Grade.HasValue)
                    .ToList();

                if (gradedFiles.Any())
                {
                    studentSubject.Grade = (int)gradedFiles.Average(gf => gf.Grade.Value);
                }
                else
                {
                    studentSubject.Grade = null;
                }

                await _userRepository.UpdateAsync(user);
            }
        }

        TempData["Success"] = "File graded successfully.";
        return RedirectToAction(nameof(Details), new { id = gradedFile.HomeworkId });
    }
}