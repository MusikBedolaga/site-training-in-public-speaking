using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser
{
    public class LectureModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public LectureModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
        
        public Lecture? SelectedLecture { get; set; }
        
        public int? NextLectureId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            SelectedLecture = await _dataBaseService.GetLectureByIdAsync(id);
            if (SelectedLecture == null)
            {
                return NotFound();
            }

            var next = await _dataBaseService.GetNextLectureAsync(
                SelectedLecture.CourseId, 
                SelectedLecture.Id);
            NextLectureId = next?.Id;

            return Page();
        }
    }
}