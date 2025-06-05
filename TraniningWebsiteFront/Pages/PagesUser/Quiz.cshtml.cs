using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class QuizModel : PageModel
{
    private readonly DataBaseService _dataBaseService;

    public QuizModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }

    [BindProperty]
    public string Answer { get; set; }
    
    public Quiz? SelectedQuiz { get; set; }
    
    public int? NextQuizId { get; set; }
    
    public bool? IsAnswerCorrect { get; set; }
    public bool IsSubmitted { get; set; }
    public bool? IsCorrectAnswer { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        SelectedQuiz = await _dataBaseService.GetQuizByIdAsync(id);

        if (SelectedQuiz == null)
            return NotFound();

        var next = await _dataBaseService.GetNextQuizAsync(
            SelectedQuiz.CourseId,
            SelectedQuiz.Id);
        NextQuizId = next?.Id;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Answer))
        {
            return Page();
        }
        
        SelectedQuiz = await _dataBaseService.GetQuizByIdAsync(id);

        if (SelectedQuiz == null)
        {
            return NotFound();
        }
        
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null)
        {
            return RedirectToPage("/Auth+Regist/Login");
        }

        var userId = int.Parse(userIdClaim.Value);
        
        await _dataBaseService.AddQuizAttemptAsync(SelectedQuiz.Id, Answer, userId);

        IsCorrectAnswer = (Answer.Trim().Equals(SelectedQuiz.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase));
        IsSubmitted = true;
        
        bool isSubscribed = await _dataBaseService.IsUserSubscribedToCourseAsync(userId, SelectedQuiz.CourseId);
        if (!isSubscribed)
        {
            TempData["SubscriptionAlert"] = "Для прохождения теста необходимо подписаться на курс.";
            return RedirectToPage("/PagesUser/Course", new { id = SelectedQuiz.CourseId });
        }
        
        var next = await _dataBaseService.GetNextQuizAsync(
            SelectedQuiz.CourseId,
            SelectedQuiz.Id);
        NextQuizId = next?.Id;
        
        return Page();
    }
}