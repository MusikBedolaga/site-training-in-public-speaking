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

    public List<Course> Courses { get; set; } = new();
    public List<User> Users { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public string SearchQuery { get; set; }

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            Courses = await _elasticSearchService.SearchCoursesAsync(SearchQuery);
        }
        else
        {
            Courses = await _dataBaseService.GetAllCoursesForAdminAsync();
        }
        
        Users = await _dataBaseService.GetAllUsersAsync();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _dataBaseService.DeleteCourseByIdAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostBanUserAsync(int userId, DateTime banEndDate, string banReason)
    {
        try
        {
            if (banEndDate <= DateTime.UtcNow)
            {
                TempData["Error"] = "���� ��������� ���� ������ ���� � �������";
                return RedirectToPage();
            }

            await _dataBaseService.BanUserAsync(userId, banEndDate, banReason);
            TempData["Message"] = "������������ ������� �������";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"������: {ex.Message}";
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnbanUserAsync(int userId)
    {
        try
        {
            await _dataBaseService.UnbanUserAsync(userId);
            TempData["Message"] = "������������ ������� ��������";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"������: {ex.Message}";
        }
        return RedirectToPage();
    }
   

    public async Task<IActionResult> OnPostDeleteUserAsync(int userId)
    {
        try
        {
            var result = await _dataBaseService.DeleteUserWithDependenciesAsync(userId);
            if (result)
            {
                TempData["Message"] = "������������ ������� ������";
            }
            else
            {
                TempData["Error"] = "�� ������� ������� ������������";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"������: {ex.Message}";
        }
        return RedirectToPage();
    }

    
}