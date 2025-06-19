using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Models.TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Services.PDF;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class GenerateCertificatModel : PageModel
    {
        private readonly DataBaseService _context;
        private readonly PdfCertificateGenerator _pdfGenerator;

        public GenerateCertificatModel(
            DataBaseService context,
            PdfCertificateGenerator pdfGenerator)
        {
            _context = context;
            _pdfGenerator = pdfGenerator;
        }

        [BindProperty]
        public Certificate Certificate { get; set; } = new Certificate();

        [FromQuery]
        public int CourseId { get; set; }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            Course = await _context.GetCourseByIdAsync(courseId);
            if (Course == null) return NotFound();

            var existingCert = await _context.GetCertificateByCourseIdAsync(courseId);
            if (existingCert != null)
            {
                Certificate = existingCert;
            }
            else
            {
                Certificate.CourseId = courseId;
                Certificate.Title = $"Сертификат курса {Course.Name}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            // Получаем курс для проверки
            Course = await _context.GetCourseByIdAsync(Certificate.CourseId);
            if (Course == null) return NotFound();

            // Генерация PDF
            var pdfBytes = _pdfGenerator.Generate(
                studentName: "Лучший Студент", // Замените на реальные данные
                courseName: Course.Name,
                issueDate: DateTime.Now
            );

            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                ModelState.AddModelError("", "Ошибка генерации PDF");
                return Page();
            }

            // Сохраняем или обновляем сертификат
            var existingCert = await _context.GetCertificateByCourseIdAsync(Certificate.CourseId);

            if (existingCert != null)
            {
                existingCert.Title = Certificate.Title;
                existingCert.Description = Certificate.Description;
                existingCert.TemplatePath = Certificate.TemplatePath;
                existingCert.PdfContent = pdfBytes;
                existingCert.IssueDate = DateTime.UtcNow;
                await _context.UpdateCertificateAsync(existingCert);
            }
            else
            {
                Certificate.PdfContent = pdfBytes;
                Certificate.IssueDate = DateTime.UtcNow;
                await _context.AddCertificateAsync(Certificate);
            }

            return RedirectToPage("/PagesTeacher/ExistCourse", new { id = Certificate.CourseId });
        }
    }
}