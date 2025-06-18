using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;
using System.Threading.Tasks;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class CreateLectureModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public CreateLectureModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [BindProperty]
        public Lecture Lecture { get; set; } = new Lecture();

        [FromQuery] // �������� �������� �� query string
        public int CourseId { get; set; }

        public void OnGet()
        {
            // ��� �������
            Console.WriteLine($"OnGet: CourseId = {CourseId}");
        }

        public async Task<IActionResult> OnPostAsync(int courseId)
        {
            // ���������� �������� �� �����
            CourseId = courseId;
            Console.WriteLine($"OnPost: CourseId = {CourseId}");

            if (CourseId <= 0)
            {
                Console.WriteLine("������: �������� ID �����");
                return RedirectToPage("/Error");
            }

            var course = await _dataBaseService.GetCourseByIdAsync(CourseId);
            if (course == null)
            {
                Console.WriteLine($"������: ���� {CourseId} �� ������");
                return RedirectToPage("/Error");
            }

            Lecture.CourseId = CourseId;
            await _dataBaseService.AddLecturerAsync(Lecture);
            Console.WriteLine($"������ ��������� � ����� {CourseId}");

            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = CourseId });
        }
    }
}