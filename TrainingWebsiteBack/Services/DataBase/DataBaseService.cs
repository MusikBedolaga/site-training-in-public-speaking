using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainingWebsiteBack.Models;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainingWebsiteBack.Services.DataBase
{
    public class DataBaseService
    {
        private readonly AppDbContext _context;
        
        private static readonly Lazy<DataBaseService> _instance = 
            new Lazy<DataBaseService>(() => new DataBaseService());
        
        public static DataBaseService Instance => _instance.Value;
        
        private DataBaseService()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            );
            _context = new AppDbContext(optionsBuilder.Options);
        }
        
        //MARK: User
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        
        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        
        public async Task UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
    
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
    
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
        //MARK: Reviews
        public async Task AddNewReviewAsync(string content)
        {
            var review = new Reviews
            {
                Content = content,
            };
            
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return;
            
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}