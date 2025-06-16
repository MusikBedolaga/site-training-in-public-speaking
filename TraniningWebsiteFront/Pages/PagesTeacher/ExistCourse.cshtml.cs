using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class CourseModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public CourseModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
        public List<Lecture> Lectures { get; set; }
        public List<Quiz> Quizs { get; set; }
        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _dataBaseService.GetCourseByIdAsync(id);
            Lectures = await _dataBaseService.GetLecturesCurrentCourse(id);
            Quizs = await _dataBaseService.GetQuizzesCurrentCourse(id);
            if (Course == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Course.Id == 0)
            {
                return NotFound();  // Или возвращаем ошибку, если Id равен 0
            }

            await _dataBaseService.UpdateCourseAsync(Course);
            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = Course.Id });
        }
        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //        return Page();

        //    _dataBaseService.UpdateCourse(Course);
        //    return RedirectToPage("/PagesTeacher/Course", new { id = Course.Id });
        //}
    }
}
