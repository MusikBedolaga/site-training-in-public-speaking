using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class FeedbackModel : PageModel
{
    private readonly DataBaseService _dataBaseService;
    
    public FeedbackModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }
        
    
    [BindProperty]
    [Required(ErrorMessage = "Это поле не может быть пустым")]
    public string FeedbackText { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        await _dataBaseService.AddNewReviewAsync(FeedbackText);

        return RedirectToPage();
    }
}