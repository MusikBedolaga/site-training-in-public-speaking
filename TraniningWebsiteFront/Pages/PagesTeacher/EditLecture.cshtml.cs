using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class EditLectureModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public EditLectureModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [FromQuery]
        public int lectureId { get; set; }

        [BindProperty]
        public Lecture Lecture { get; set; }= new Lecture();


        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine("asdasdasdasd", lectureId);
            Lecture = await _dataBaseService.GetLectureByIdAsync(lectureId);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Lecture.Id = lectureId;
            await _dataBaseService.UpdateLectureAsync(Lecture);
            Lecture = await _dataBaseService.GetLectureByIdAsync(lectureId);
            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = Lecture.CourseId });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int lectureId, int courseId)
        {
            try
            {
                await _dataBaseService.DeleteLectureAsync(lectureId);
                return RedirectToPage("/PagesTeacher/ExistCourse", new { id = courseId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при удалении: {ex.Message}");
                return Page();
            }
        }
    }
}
