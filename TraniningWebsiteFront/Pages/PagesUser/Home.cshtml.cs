using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class HomeModel : PageModel
{
    private readonly DataBaseService _dataBaseService;

    public HomeModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }

    public List<Course> Courses { get; set; } = new();

    public async Task OnGetAsync()
    {
        Courses = await _dataBaseService.GetAllCoursesAsync();
    }
}
