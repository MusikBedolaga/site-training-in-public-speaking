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

    public Quiz? SelectedQuiz { get; set; }
    
    public int? NextQuizId { get; set; }

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
}