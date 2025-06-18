using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class CreateQuizModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public CreateQuizModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        [FromQuery]
        public int CourseId { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            // Проверяем существование курса
            var course = await _dataBaseService.GetCourseByIdAsync(CourseId);
            //if (course == null)
            //{
            //    ModelState.AddModelError("", $"Курс с ID {CourseId} не найден!");
            //    return Page();
            //}

            Quiz.CourseId = CourseId;
            await _dataBaseService.AddQuizAsync(Quiz);
            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = CourseId });
        }
    }
}
