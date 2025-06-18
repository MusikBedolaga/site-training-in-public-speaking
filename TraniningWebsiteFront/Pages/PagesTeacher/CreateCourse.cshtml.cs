using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class CreateCourseModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public CreateCourseModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [BindProperty]
        public List<Lecture> Lectures { get; set; } = new();
        [BindProperty]
        public List<Quiz> Quizs { get; set; } = new();
        [BindProperty]
        public Course Course { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                Course = await _dataBaseService.GetCourseByIdAsync(id.Value);
                if (Course != null)
                {
                    Lectures = await _dataBaseService.GetLecturesCurrentCourse(id.Value);
                    Quizs = await _dataBaseService.GetQuizzesCurrentCourse(id.Value);
                }
            }
            else
            {
                var userIdClaim = User.FindFirst("UserId");
                var UserId = int.Parse(userIdClaim.Value);
                var courses = await _dataBaseService.GetAllUserCoursesAsync(UserId);
                Course = courses.LastOrDefault();

                if (Course != null)
                {
                    Lectures = await _dataBaseService.GetLecturesCurrentCourse(Course.Id);
                    Quizs = await _dataBaseService.GetQuizzesCurrentCourse(Course.Id);
                }
                else
                {
                    // Если ни один курс не найден — создаём временный "пустой"
                    Course = new Course { Name = "Новый курс" };
                    Lectures = new List<Lecture>();
                    Quizs = new List<Quiz>();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdClaim = User.FindFirst("UserId");
            var UserId = int.Parse(userIdClaim.Value);

            Course.IsCompleted = false;
            Course.CreatorId = UserId;

            // ��������� ���� � �������� ��������� ������++
            var createdCourse = await _dataBaseService.AddCourseAsync(Course);

            // �������������� � ID ���������� �����
            return RedirectToPage("/PagesTeacher/CreateCourse", new { id = createdCourse.Id });
        }
    }
}
