using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public class Achievement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Image { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Description { get; set; }
    
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}