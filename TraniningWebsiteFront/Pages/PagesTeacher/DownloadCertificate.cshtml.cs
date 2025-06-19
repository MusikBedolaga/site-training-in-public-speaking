using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class DownloadCertificateModel : PageModel
{
    private readonly DataBaseService _dataBaseService;

    public DownloadCertificateModel(DataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }

    [BindProperty(SupportsGet = true)]
    public int CourseId { get; set; }

    public byte[]? CertificateBytes { get; set; }
    public string CourseName { get; set; } = string.Empty;



    public async Task<IActionResult> OnGetAsync()
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null) return RedirectToPage("/Auth+Regist/Login");

        var certificate = await _dataBaseService.GetCertificateByCourseIdAsync(CourseId);
        if (certificate?.PdfContent == null)
            return NotFound();

        return File(certificate.PdfContent, "application/pdf", $"Certificate_{CourseId}.pdf");
    }
}