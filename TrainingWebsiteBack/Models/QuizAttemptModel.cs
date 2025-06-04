using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class QuizAttempt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 

    [Required]
    [ForeignKey("Quiz")]
    public int QuizId { get; set; }

    public Quiz Quiz { get; set; }

    [Required]
    [MaxLength(256)]
    public string Attempt { get; set; }

    [Required]
    [MaxLength(256)]
    public string Description { get; set; }
}