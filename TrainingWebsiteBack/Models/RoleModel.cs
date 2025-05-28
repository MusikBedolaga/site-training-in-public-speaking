using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingWebsiteBack.Models;

public enum RoleEnum
{
    User,
    Teacher,
    Admin
};

public class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public RoleEnum Name { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
}