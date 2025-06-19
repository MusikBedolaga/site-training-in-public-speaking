using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Services.PDF;
using System.Net.Mail;
using System.Net;

namespace TraniningWebsiteFront.Pages.PagesUser;

public class DownloadCertificateModel : PageModel
{
    private readonly DataBaseService _dataBaseService;
    private readonly PdfCertificateGenerator _pdfGenerator;

    public DownloadCertificateModel(DataBaseService dataBaseService, PdfCertificateGenerator pdfGenerator)
    {
        _dataBaseService = dataBaseService;
        _pdfGenerator = pdfGenerator;
    }

    [BindProperty(SupportsGet = true)]
    public int CourseId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null)
            return RedirectToPage("/Auth+Regist/Login");

        var user = await _dataBaseService.GetUserByIdAsync(int.Parse(userIdClaim.Value));
        if (user == null)
            return Unauthorized();

        var course = await _dataBaseService.GetCourseByIdAsync(CourseId);
        if (course == null)
            return NotFound();

        var pdf = _pdfGenerator.Generate(user.Name, course.Name, DateTime.Now);

        await SendCertificateToEmailAsync(user.Email, pdf);

        TempData["Message"] = "���������� ��������� �� ��� email.";
<<<<<<< HEAD
        return RedirectToPage("/PagesUser/Course", new { id = CourseId }); // �������� Razor-�������� �������������
=======
        return Page(); // �������� Razor-�������� �������������
>>>>>>> createfuncemail
    }

    private async Task SendCertificateToEmailAsync(string toEmail, byte[] pdfBytes)
    {
        var message = new MailMessage("rostislavvrublevsky@ya.ru", toEmail)
        {
            Subject = "��� ����������",
            Body = "������������! ��� ���������� �� ����� ��������� � ������.",
            IsBodyHtml = false
        };

        message.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "Certificate.pdf", "application/pdf"));


        using var smtp = new SmtpClient("smtp.yandex.ru", 587)  // �������� �� ���� ������
        {
            Credentials = new NetworkCredential("rostislavvrublevsky@yandex.ru", "onydqpepqzgmkodk"),
            EnableSsl = true
        };

        await smtp.SendMailAsync(message);
    }
}
