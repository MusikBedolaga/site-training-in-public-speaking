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
    
    public bool IsSubscribed { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            IsSubscribed = await _dataBaseService.IsUserSubscribedToCourseAsync(userId, id);
        }

        
        SelectedCourse = await _dataBaseService.GetCourseByIdAsync(id);
        if (SelectedCourse == null)
        {
            return NotFound();
        }

        Lectures = await _dataBaseService.GetLecturesCurrentCourse(id);
        Quizzes = await _dataBaseService.GetQuizzesCurrentCourse(id);

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return RedirectToPage("/Auth+Regist/Login");
        }

        var success = await _dataBaseService.SubscribeToCourseAsync(userId, id);

        if (!success)
        {
            return NotFound();
        }

        SelectedCourse = await _dataBaseService.GetCourseByIdAsync(id);
        Lectures = await _dataBaseService.GetLecturesCurrentCourse(id);
        Quizzes = await _dataBaseService.GetQuizzesCurrentCourse(id);

        return RedirectToPage(new { id });
    }

}