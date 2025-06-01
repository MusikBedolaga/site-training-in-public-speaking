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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        public async Task UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
    
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> UpdateUserGetBoolAsync(User user)
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка обновления: " + ex.Message);
                return false;
            }
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
        
        //MARK: Course
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }
        
        public async Task DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
        
        //MARK: Lecture
        public async Task AddLecturerAsync(Lecture lecturer)
        {
            if (lecturer == null) throw new ArgumentNullException(nameof(lecturer));
            
            _context.AddAsync(lecturer);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<Lecture>> GetLecturesCurrentCourse(int courseId)
        {
            return await _context.Lectures
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Lecture?> GetLectureByIdAsync(int lectureId)
        {
            return await _context.Lectures.FindAsync(lectureId);
        }
        
        public async Task<Lecture?> GetNextLectureAsync(int courseId, int currentLectureId)
        {
            return await _context.Lectures
                .Where(l => l.CourseId == courseId && l.Id > currentLectureId)
                .OrderBy(l => l.Id)
                .FirstOrDefaultAsync();
        }
        
        
        //MARK: Quiz 
        public async Task AddQuizAsync(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));
            
            _context.AddAsync(quiz);
            await _context.SaveChangesAsync();  
        }
        
        public async Task<List<Quiz>> GetQuizzesCurrentCourse(int courseId)
        {
            return await _context.Quizzes
                                 .Where(q => q.CourseId == courseId)
                                 .ToListAsync();
        }

        public async Task<Quiz?> GetQuizByIdAsync(int quizId)
        {
            return await _context.Quizzes.FindAsync(quizId);
        }
        
        public async Task<Quiz?> GetNextQuizAsync(int courseId, int currentQuizId)
        {
            return await _context.Quizzes
                .Where(q => q.CourseId == courseId && q.Id > currentQuizId)
                .OrderBy(q => q.Id)
                .FirstOrDefaultAsync();
        }
    }
}