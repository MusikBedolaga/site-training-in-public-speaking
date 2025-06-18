using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesAdmin;

public class HomeModel : PageModel
{
    private readonly DataBaseService _dataBaseService;
    private readonly ElasticSearchService _elasticSearchService;

    public HomeModel(DataBaseService dataBaseService, ElasticSearchService elasticSearchService)
    {
        _dataBaseService = dataBaseService;
        _elasticSearchService = elasticSearchService;
    }

    public List<Course> Courses { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string SearchQuery { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            Courses = await _elasticSearchService.SearchCoursesAsync(SearchQuery);
        }
        else
        {
            Courses = await _dataBaseService.GetAllCoursesForAdminAsync();
        }
        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _dataBaseService.DeleteCourseByIdAsync(id);
        return RedirectToPage();
    }
}
