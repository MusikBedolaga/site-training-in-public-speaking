using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Models;

namespace TraniningWebsiteFront.Pages;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly NetworkClient _networkClient;
    
    public RegisterModel(AppDbContext context, NetworkClient networkClient)
    {
        _context = context;
        _networkClient = networkClient;
    }

    public IList<SelectListItem> Roles { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "ФИО обязательно")]
    public string FullName { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [BindProperty]
    public IFormFile Photo { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Выберите роль")]
    public int RoleId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Roles = await _context.Roles
            .Select(r => new SelectListItem 
            { 
                Value = r.Id.ToString(), 
                Text = r.Name.ToString() 
            })
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }
        
        if (await _context.Users.AnyAsync(u => u.Email == Email))
        {
            ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
            await OnGetAsync();
            return Page();
        }
        
        string photoPath = null;
        if (Photo != null && Photo.Length > 0)
        {
            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Photo.CopyToAsync(stream);
            }

            photoPath = $"/uploads/{uniqueFileName}";
        }
        
        var user = new User
        {
            Name = FullName,
            Email = Email,
            Password = Password,
            Photo = photoPath,
            RoleId = RoleId
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _networkClient.SendCommandAsync($"NEW_USER:{Email}");

        return RedirectToPage("/Auth+Regist/Login");
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

