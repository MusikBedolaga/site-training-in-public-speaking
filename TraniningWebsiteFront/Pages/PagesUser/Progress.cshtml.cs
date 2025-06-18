using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class ProgressModel : PageModel
{
    public List<AchievementViewModel> Achievements { get; set; } = new();
    
    public void OnGet()
    {
        Achievements = new List<AchievementViewModel>
        {
            new AchievementViewModel { ImageUrl = "/images/medal1.png", Description = "Прошел 5 тестов" },
            new AchievementViewModel { ImageUrl = "/images/medal2.png", Description = "Заполнил профиль" },
            new AchievementViewModel { ImageUrl = "/images/medal3.png", Description = "Подписан на 3 курса" },
        };
    }
}

public class AchievementViewModel
{
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}