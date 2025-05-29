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
        
        public DataBaseService(AppDbContext context)
        {
            _context = context;
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
        
        public async Task<User?> GetUserByCredentialsAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
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
        
        //MARK: Role
        public async Task<Role> GetRoleAsync(User user)
        {
            return await _context.Roles.FindAsync(user.RoleId);
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