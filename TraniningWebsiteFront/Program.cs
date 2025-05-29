using TrainingWebsiteBack.Services.DataBase;
using Microsoft.EntityFrameworkCore;
using TraniningWebsiteFront;

var builder = WebApplication.CreateBuilder(args);

// Регистрация NetworkClient
builder.Services.AddSingleton<NetworkClient>(_ => new NetworkClient("127.0.0.1", 8000));

// AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Подключение к базе
builder.Services.AddScoped<DataBaseService>();


builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Registration", "");
    options.Conventions.AddPageRoute("/Index", "IndexPage");
});

var app = builder.Build();

// Тестовый запрос при запуске
// var networkClient = app.Services.GetRequiredService<NetworkClient>();
// var response = await networkClient.SendCommandAsync("GET_USERS");
// Console.WriteLine($"Ответ сервера: {response}");

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();