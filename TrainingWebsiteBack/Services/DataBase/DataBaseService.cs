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
        public async Task<List<Course>> GetAllUserCoursesAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.CreatedCourses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return new List<Course>();

            // Объединить созданные и подписанные курсы без дубликатов
            var allCourses = user.CreatedCourses
                .Concat(user.SubscribedCourses)
                .Distinct()
                .ToList();

            return allCourses;
        }

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

        public async Task UpdateCourseAsync(Course course)
        {
            try
            {
                // Найти существующий курс по ID
                var existingCourse = await _context.Courses.FindAsync(course.Id);

                if (existingCourse == null)
                {
                    // Если курс не найден, логируем и выбрасываем исключение
                    Console.WriteLine($"Курс с ID {course.Id} не найден.");
                    throw new ArgumentException("Курс не найден");
                }

                // Обновляем данные курса
                Console.WriteLine($"Обновляем курс: старое название - {existingCourse.Name}, новое название - {course.Name}");
                existingCourse.Name = course.Name;
                existingCourse.Description = course.Description;

                // Уведомляем контекст о том, что объект был изменен
                _context.Entry(existingCourse).State = EntityState.Modified;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
                Console.WriteLine("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                // Логируем исключение
                Console.WriteLine($"Ошибка при обновлении курса: {ex.Message}");
                throw;
            }
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        //MARK: CourseSubscriptions
        public async Task<bool> SubscribeToCourseAsync(int userId, int courseId)
        {
            var user = await _context.Users.Include(u => u.SubscribedCourses).FirstOrDefaultAsync(u => u.Id == userId);
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (user == null || course == null)
                return false;

            if (!user.SubscribedCourses.Any(c => c.Id == courseId))
            {
                user.SubscribedCourses.Add(course);
                await _context.SaveChangesAsync();
            }

            return true;
        }
        
        public async Task<bool> IsUserSubscribedToCourseAsync(int userId, int courseId)
        {
            var user = await _context.Users
                .Include(u => u.SubscribedCourses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;
            return user.SubscribedCourses.Any(c => c.Id == courseId);
        }

        
        //MARK: Lecture
        public async Task AddLecturerAsync(Lecture lecturer)
        {
            if (lecturer == null) throw new ArgumentNullException(nameof(lecturer));
            
            await _context.Lectures.AddAsync(lecturer);
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

        public async Task UpdateLectureAsync(Lecture lecture)
        {
            try
            {
                // Найти существующий курс по ID
                var existingLecture = await _context.Lectures.FindAsync(lecture.Id);

                if (existingLecture == null)
                {
                    // Если курс не найден, логируем и выбрасываем исключение
                    Console.WriteLine($"Лекция с ID {lecture.Id} не найден.");
                    throw new ArgumentException("Курс не найден");
                }

                // Обновляем данные курса
                Console.WriteLine($"Обновляем лекции: старое название - {existingLecture.Title}, новое название - {lecture.Title}");
                existingLecture.Title = lecture.Title;
                existingLecture.Content = lecture.Content;

                // Уведомляем контекст о том, что объект был изменен
                _context.Entry(existingLecture).State = EntityState.Modified;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
                Console.WriteLine("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                // Логируем исключение
                Console.WriteLine($"Ошибка при обновлении лекции: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteLectureAsync(int lectureId)
        {
            var lecture = await _context.Lectures.FindAsync(lectureId);
            if (lecture != null)
            {
                _context.Lectures.Remove(lecture);
                await _context.SaveChangesAsync();
            }
        }

        //MARK: Quiz 
        public async Task AddQuizAsync(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));
            
            _context.Quizzes.AddAsync(quiz);
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
        public async Task UpdateQuizAsync(Quiz quiz)
        {
            try
            {
                // Найти существующий quiz по ID
                var existingQuiz = await _context.Quizzes.FindAsync(quiz.Id);

                if (existingQuiz == null)
                {
                    // Если quiz не найден, логируем и выбрасываем исключение
                    Console.WriteLine($"Test с ID {quiz.Id} не найден.");
                    throw new ArgumentException("Курс не найден");
                }

                // Обновляем данные quiz
                Console.WriteLine($"Обновляем test: старое название - {existingQuiz.Title}, новое название - {quiz.Title}");
                existingQuiz.Title = quiz.Title;
                existingQuiz.Content = quiz.Content;
                existingQuiz.CorrectAnswer = quiz.CorrectAnswer;

                // Уведомляем контекст о том, что объект был изменен
                _context.Entry(existingQuiz).State = EntityState.Modified;

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
                Console.WriteLine("Изменения сохранены.");
            }
            catch (Exception ex)
            {
                // Логируем исключение
                Console.WriteLine($"Ошибка при обновлении testa: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }

        //MARK: QuizAttempt
        public async Task AddQuizAttemptAsync(QuizAttempt attempt)
        {
            if (attempt == null) throw new ArgumentNullException(nameof(attempt));
            
            await _context.AddAsync(attempt);
            await _context.SaveChangesAsync();
        }
        
        public async Task AddQuizAttemptAsync(
            int quizId,
            string answer,
            int userId,
            string description = "")
        {
            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                Attempt = answer,
                UserId = userId,
                Description = description,
            };
            
            await _context.AddAsync(attempt);
            await _context.SaveChangesAsync();
        }
        
    }
}