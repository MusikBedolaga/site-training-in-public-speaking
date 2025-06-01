using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class UserCourse
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public double Progress { get; set; } = 0.0;
}