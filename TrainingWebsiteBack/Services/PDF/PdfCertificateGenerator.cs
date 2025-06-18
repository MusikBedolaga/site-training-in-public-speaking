using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
    using QuestPDF.Infrastructure;
    using QuestPDF.Helpers;

namespace TrainingWebsiteBack.Services.PDF
{
    public class PdfCertificateGenerator
    {
        public byte[] Generate(string studentName, string courseName, DateTime issueDate)
        {
            // Настройка документа
            QuestPDF.Settings.License = LicenseType.Community; // Бесплатная лицензия

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4); // Формат A4
                    page.Margin(2, Unit.Centimetre); // Отступы 2 см

                    // Заголовок
                    page.Header()
                        .AlignCenter()
                        .Text("Сертификат о прохождении курса")
                        .Bold().FontSize(24);

                    // Основное содержимое
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Text($"Студент: {studentName}");
                            column.Item().Text($"Курс: {courseName}");
                            column.Item().Text($"Дата выдачи: {issueDate:dd.MM.yyyy}");
                        });

                    // Подвал (опционально)
                    page.Footer()
                        .AlignCenter()
                        .Text("© Ваша образовательная платформа");
                });
            });

            return document.GeneratePdf(); // Конвертируем в byte[]
        }
    }
}
