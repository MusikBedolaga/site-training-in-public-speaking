using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

public class UsersModel : PageModel
{
    private readonly AppDbContext _context;

    public List<User> Users { get; set; }

    public UsersModel(AppDbContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Users = _context.Users.ToList();
    }
}