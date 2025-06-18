using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrainingWebsiteBack.Services.DataBase;
using TrainingWebsiteBack.Services.PDF;
using TraniningWebsiteFront;

var builder = WebApplication.CreateBuilder(args);

// NetworkClient
builder.Services.AddSingleton<NetworkClient>(_ => new NetworkClient("127.0.0.1", 8000));

// DbContext и DataBaseService
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Elasticsearch Service
builder.Services.AddSingleton(new ElasticSearchService("http://localhost:9200"));

// Регистрируем DataBaseService, учитывая ElasticSearchService
builder.Services.AddScoped<DataBaseService>(sp =>
{
    var context = sp.GetRequiredService<AppDbContext>();
    var elastic = sp.GetRequiredService<ElasticSearchService>();
    return new DataBaseService(context, elastic);
});

// Регистрируем PdfCertificateGenerator
builder.Services.AddScoped<PdfCertificateGenerator>();

// RazorPages + маршруты
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Auth+Regist/Registration", "");
    options.Conventions.AddPageRoute("/Index", "IndexPage");
});

// Аутентификация с куками
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth+Regist/Login";
        options.Cookie.Name = "UserAuthCookie";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();