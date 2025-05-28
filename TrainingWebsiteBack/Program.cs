using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Models;

namespace TrainingWebsiteBack;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Проверка подключения...");
        
        var dbService = DataBaseService.Instance;

        await dbService.AddNewReviewAsync("Крутой сайт");
        await dbService.AddNewReviewAsync("Вау!!! Оно работает");
    }
}