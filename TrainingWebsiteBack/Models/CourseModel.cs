using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("Creator")]
    public int CreatorId { get; set; }
    
    public User Creator { get; set; }

    [Required]
    public bool IsCompleted { get; set; } = false;
    
    public ICollection<User> Subscribers { get; set; } = new List<User>();
    
    public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
    
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}