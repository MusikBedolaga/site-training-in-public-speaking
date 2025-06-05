using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages;

public class LoginModel : PageModel
{
    private readonly DataBaseService _dataBaseService;
    private readonly NetworkClient _networkClient;
    
    public LoginModel(DataBaseService dataBaseService, NetworkClient networkClient)
    {
        _networkClient = networkClient;
        _dataBaseService = dataBaseService;
    }
    
    [BindProperty]
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var user = await _dataBaseService.GetUserByCredentialsAsync(Email, Password);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Неверный email или пароль");
            return Page();
        }

        var role = await _dataBaseService.GetRoleAsync(user);

        // надо будет поменять страницы
        return role.Name switch
        {
            RoleEnum.User => RedirectToPage("/PagesUser/Home"),
            RoleEnum.Teacher => RedirectToPage("/PagesTeacher/Home"),
            RoleEnum.Admin => RedirectToPage("/PagesAdmin/Home"),
            _ => RedirectToPage("/Index")
        };
    }
}

