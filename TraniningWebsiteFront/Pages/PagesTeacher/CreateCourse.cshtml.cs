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
                Lectures = await _dataBaseService.GetLecturesCurrentCourse(id.Value);
                Quizs = await _dataBaseService.GetQuizzesCurrentCourse(id.Value);
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
                }
            }

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var userIdClaim = User.FindFirst("UserId");
        //    var UserId = int.Parse(userIdClaim.Value);

        //    Course.IsCompleted = false;
        //    Course.CreatorId = UserId;
        //    // Добавляем курс в базу данных

        //    await _dataBaseService.AddCourseAsync(Course);
        //    return RedirectToPage("/PagesTeacher/CreateLecture", new { id = Course.Id });
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdClaim = User.FindFirst("UserId");
            var UserId = int.Parse(userIdClaim.Value);

            Course.IsCompleted = false;
            Course.CreatorId = UserId;

            // Добавляем курс и получаем созданный объект
            var createdCourse = await _dataBaseService.AddCourseAsync(Course);

            // Перенаправляем с ID созданного курса
            return RedirectToPage("/PagesTeacher/CreateCourse", new { id = createdCourse.Id });
        }
    }
}
