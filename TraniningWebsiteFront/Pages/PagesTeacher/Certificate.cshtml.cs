using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrainingWebsiteBack.Models.TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TraniningWebsiteFront.Pages.PagesTeacher
{
    public class CertificateModel : PageModel
    {
        private readonly DataBaseService _dataBaseService;

        public CertificateModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public Certificate Certificate { get; set; }

    }
}
