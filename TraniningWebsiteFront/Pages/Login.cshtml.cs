using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TraniningWebsiteFront.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public void OnPost()
    {
        // Логика входа (сравнение с БД, сессия и т.п.)
    }
}

