using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class HomeModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public HomeModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
        public List<Course> Courses { get; set; }




        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirst("UserId");
            var UserId = int.Parse(userIdClaim.Value);

            if (UserId == null)
            {
                return RedirectToPage("/Auth+Regist/Login");
            }

            Courses = await _dataBaseService.GetAllUserCoursesAsync(UserId);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var course = await _dataBaseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            await _dataBaseService.DeleteCourseAsync(id);
            return RedirectToPage(); // ????????? ????????
        }
    }
}