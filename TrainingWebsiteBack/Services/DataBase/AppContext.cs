using Microsoft.EntityFrameworkCore;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Models.TrainingWebsiteBack.Models;

namespace TrainingWebsiteBack.Services.DataBase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Reviews> Reviews { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Quiz>  Quizzes { get; set; }
    public DbSet<UserLecture> UserLectures { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи Role (1) → Users (Many)
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

        // Настройка связи User (1) → CreatedCourses (Many) (курсы, созданные пользователем)
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedCourses)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи многие-ко-многим между User и Course (подписчики)
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Subscribers)
            .WithMany(u => u.SubscribedCourses)
            .UsingEntity<Dictionary<string, object>>(
                "CourseSubscriptions",
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                j => j.HasKey("UserId", "CourseId"));
        
        // Настройка Reviews (отзывов)
        modelBuilder.Entity<Reviews>(entity =>
        {
            entity.ToTable("Reviews"); // Явное имя таблицы
            entity.Property(r => r.Content).HasColumnType("text"); // Для PostgreSQL
            entity.Property(r => r.CreatedAt)
                .HasDefaultValueSql("NOW()") // Для PostgreSQL
                .ValueGeneratedOnAdd();
        });
        
        // Настройка связи Course (1) → Lectures (Many)
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Lectures)
            .WithOne(l => l.Course)
            .HasForeignKey(l => l.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Настройка связи Course (1) → Quizzes (Many)
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Quizzes)
            .WithOne(q => q.Course)
            .HasForeignKey(q => q.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Настройка связи 1:M между Quiz и QuizAttempt
        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Attempts)
            .WithOne(a => a.Quiz)
            .HasForeignKey(a => a.QuizId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Настройка связи M:N через UserAchievement
        modelBuilder.Entity<UserAchievement>()
            .HasKey(ua => ua.Id);

        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.UserAchievements)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserAchievement>()
            .HasOne(ua => ua.Achievement)
            .WithMany(a => a.UserAchievements)
            .HasForeignKey(ua => ua.AchievementId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Связь QuizAttempt → User
        modelBuilder.Entity<QuizAttempt>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserLecture>()
            .HasIndex(ul => new { ul.UserId, ul.LectureId })
            .IsUnique();

        modelBuilder.Entity<UserLecture>()
            .HasOne(ul => ul.User)
            .WithMany()
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLecture>()
            .HasOne(ul => ul.Lecture)
            .WithMany()
            .HasForeignKey(ul => ul.LectureId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<QuizAttempt>().ToTable("QuizAttempt");

        modelBuilder.Entity<Certificate>()
        .HasOne(c => c.Course)
        .WithMany()
        .HasForeignKey(c => c.CourseId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}