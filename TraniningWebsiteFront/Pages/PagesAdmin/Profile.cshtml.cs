using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesAdmin;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly DataBaseService _dataBaseService;

    public ProfileModel(DataBaseService dataBaseService,  AppDbContext context)
    {
        _dataBaseService = dataBaseService;
        _context = context;
    }

    [BindProperty, EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    public string FullName { get; set; }

    [BindProperty]
    public IFormFile? Photo { get; set; }

    [BindProperty]
    public string? PhotoPath { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUserEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUserEmail))
            return RedirectToPage("/Auth+Regist/Login");

        var user = await _dataBaseService.GetUserByEmailAsync(currentUserEmail);
        if (user == null) return NotFound();

        Email = user.Email;
        FullName = user.Name;
        PhotoPath = user.Photo;

        return Page();
    }

    public async Task<IActionResult> OnPostChangePhotoAsync()
    {
        var currentUserEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUserEmail)) return RedirectToPage("/Auth+Regist/Login");

        var user = await _dataBaseService.GetUserByEmailAsync(currentUserEmail);
        if (user == null) return NotFound();

        if (Photo != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);
            var filePath = Path.Combine("wwwroot/uploads", fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Photo.CopyToAsync(stream);
            }

            user.Photo = "/uploads/" + fileName;

            Console.WriteLine($"[Photo] Updating photo path: {user.Photo}");

            var updated = await _dataBaseService.UpdateUserGetBoolAsync(user);
            Console.WriteLine($"[Photo] Photo update result: {updated}");

            PhotoPath = user.Photo;
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateDataAsync()
    {
        if (!ModelState.IsValid)
        {
            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                var errors = entry.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"[ModelState Error] Field: {key}, Error: {error.ErrorMessage}");
                }
            }

            return Page();
        }

        var currentUserEmail = User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUserEmail)) return RedirectToPage("/Auth+Regist/Login");

        var user = await _dataBaseService.GetUserByEmailAsync(currentUserEmail);
        if (user == null) return NotFound();

        if (Email != currentUserEmail)
        {
            var existingUser = await _dataBaseService.GetUserByEmailAsync(Email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Email", "Этот email уже используется другим пользователем.");
                return Page();
            }
        }

        user.Email = Email;
        user.Name = FullName;
        
        _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        try
        {
            var rows = await _context.SaveChangesAsync();
            Console.WriteLine($"[POST] Rows affected: {rows}");

            if (rows == 0)
            {
                ModelState.AddModelError("", "Не удалось обновить данные. Попробуйте ещё раз.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[POST] Exception: " + ex.Message);
            ModelState.AddModelError("", "Ошибка при сохранении данных");
            return Page();
        }

        if (Email != currentUserEmail)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync("Cookies", principal);
        }

        return RedirectToPage();
    }
}
