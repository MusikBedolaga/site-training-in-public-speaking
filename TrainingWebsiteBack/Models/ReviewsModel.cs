using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class Reviews
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Текст отзыва обязателен")]
    [MaxLength(2000, ErrorMessage = "Отзыв не должен превышать 2000 символов")]
    [Column(TypeName = "text")]
    public string Content { get; set; }
    
    [DataType(DataType.DateTime)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}