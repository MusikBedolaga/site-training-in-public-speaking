using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.Network;

namespace TrainingWebsiteBack;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Проверка подключения...");
        
        var dbService = DataBaseService.Instance;
        var network = NetworkService.Instance;
        
        await network.StartAsync();
        
        Console.WriteLine("Сервер запущен. Нажмите Enter для остановки.");
        Console.ReadLine();
    }
}