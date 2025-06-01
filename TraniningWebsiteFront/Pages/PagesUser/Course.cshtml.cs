using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class CourseModel : PageModel
{
    private readonly DataBaseService _dataBaseService;

    public CourseModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }

    public Course? SelectedCourse { get; set; }
    
    public List<Lecture> Lectures { get; set; } = new();
    
    public List<Quiz> Quizzes { get; set; } = new();

    public int Progress { get; set; } = 0;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        SelectedCourse = await _dataBaseService.GetCourseByIdAsync(id);
        if (SelectedCourse == null)
        {
            return NotFound();
        }

        Lectures = await _dataBaseService.GetLecturesCurrentCourse(id);
        Quizzes = await _dataBaseService.GetQuizzesCurrentCourse(id);

        return Page();
    }
}