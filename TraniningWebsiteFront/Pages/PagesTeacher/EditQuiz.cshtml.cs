using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class EditQuizModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public EditQuizModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [FromQuery]
        public int quizId { get; set; }
        [BindProperty]
        public Quiz Quiz { get; set; } = new Quiz();

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine("asdasdasdasd", quizId);
            Quiz = await _dataBaseService.GetQuizByIdAsync(quizId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Quiz.Id = quizId;
            await _dataBaseService.UpdateQuizAsync(Quiz);
            Quiz = await _dataBaseService.GetQuizByIdAsync(quizId);
            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = Quiz.CourseId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int quizId, int courseId)
        {
            try
            {
                await _dataBaseService.DeleteQuizAsync(quizId);
                return RedirectToPage("/PagesTeacher/ExistCourse", new { id = courseId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении: {ex.Message}");
                return Page();
            }
        }
    }
}
