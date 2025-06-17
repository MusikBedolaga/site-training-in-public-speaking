using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesAdmin;

public class HomeModel : PageModel
{
    private readonly DataBaseService _dataBaseService;

    public HomeModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }
    
    public List<Course> Courses { get; set; }
    
    public async Task<IActionResult> OnGetAsync ()
    {
        Courses = new List<Course>();
        
        Courses = await _dataBaseService.GetAllCoursesForAdminAsync();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _dataBaseService.DeleteCourseByIdAsync(id);
        return RedirectToPage();
    }
}