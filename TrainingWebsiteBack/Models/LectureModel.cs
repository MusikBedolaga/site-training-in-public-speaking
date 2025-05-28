using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class Lecture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Course")]
    public int CourseId { get; set; }
    
    [Required]
    public Course Course { get; set; }
    
    [Required]
    [MaxLength(256, ErrorMessage = "Название леции не должно превышать 256 символов")]
    public string Title { get; set; }

    public bool IsCompleted { get; set; } = false;
    
    [Required]
    [MaxLength(2000, ErrorMessage = "Контент леции не должен превышать 2000 символов")]
    public string Content { get; set; }

    
}