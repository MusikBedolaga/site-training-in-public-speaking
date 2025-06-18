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
        int? userId = null;

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var parsedId))
        {
            userId = parsedId;
            IsSubscribed = await _dataBaseService.IsUserSubscribedToCourseAsync(parsedId, id);
        }

        SelectedCourse = await _dataBaseService.GetCourseByIdAsync(id);
        if (SelectedCourse == null)
            return NotFound();

        Lectures = await _dataBaseService.GetLecturesCurrentCourse(id);
        Quizzes = await _dataBaseService.GetQuizzesCurrentCourse(id);

        if (userId.HasValue)
        {
            int completedLectures = await _dataBaseService.GetCompletedLectureCount(userId.Value, id);
            int passedQuizzes = await _dataBaseService.GetPassedQuizCount(userId.Value, id);

            int total = Lectures.Count + Quizzes.Count;
            int completed = completedLectures + passedQuizzes;

            Progress = total > 0 ? (int)Math.Round((double)completed / total * 100) : 0;
        }

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
            return NotFound();

        return RedirectToPage(new { id });
    }
}
