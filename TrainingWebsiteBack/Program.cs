<<<<<<< Updated upstream
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Services.Network;

namespace TrainingWebsiteBack;
=======
﻿using TrainingWebsiteBack.Services.Network;
>>>>>>> Stashed changes

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Проверка подключения...");

<<<<<<< Updated upstream
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
=======
        var network = NetworkService.Instance;

        await network.StartAsync();

        Console.WriteLine("Сервер запущен. Нажмите Enter для остановки.");
        Console.ReadLine();
>>>>>>> Stashed changes
    }
}