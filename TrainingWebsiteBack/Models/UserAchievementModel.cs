using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class UserAchievement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    public User User { get; set; }

    [ForeignKey("Achievement")]
    public int AchievementId { get; set; }
    
    public Achievement Achievement { get; set; }

    public bool IsCompleted { get; set; } = false;
}