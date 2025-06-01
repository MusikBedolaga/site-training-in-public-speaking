using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Services.Network;

namespace TrainingWebsiteBack;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Проверка подключения...");

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        using var context = new AppDbContext(optionsBuilder.Options);
        var dbService = new DataBaseService(context);
        
        var network = NetworkService.Instance;
        await network.StartAsync();
    }
}