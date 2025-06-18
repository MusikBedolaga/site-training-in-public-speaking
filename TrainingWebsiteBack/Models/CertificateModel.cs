using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingWebsiteBack.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    namespace TrainingWebsiteBack.Models
    {
            public class Certificate
            {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }

            public string TemplatePath { get; set; }

            // Только одно свойство для связи

            [Required]
            [ForeignKey("Course")]
            public int CourseId { get; set; }

            [Required]
            public Course Course { get; set; }

            [Column(TypeName = "bytea")] // Для PostgreSQL
            public byte[]? PdfContent { get; set; } // PDF в виде массива байтов

            public DateTime IssueDate { get; set; } // Дата выдачи
        }
    }
}
