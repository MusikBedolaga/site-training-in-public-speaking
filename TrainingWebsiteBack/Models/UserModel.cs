using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainingWebsiteBack.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public string? Photo { get; set; }

    //свойство для проверки пользователя на наличие бана CHC
    [Required]
    public bool IsBanned { get; set; } = false;
    
    public DateTime? BanEndDate { get; set; }
    
    public string? BanReason { get; set; }

    // Курсы, созданные пользователем
    public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
    
    // Курсы, на которые подписан пользователь
    public ICollection<Course> SubscribedCourses { get; set; } = new List<Course>();

    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
