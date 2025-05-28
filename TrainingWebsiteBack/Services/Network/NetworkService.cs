using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using TrainingWebsiteBack.Models;
using TrainingWebsiteBack.Services.DataBase;

namespace TrainingWebsiteBack.Services.Network;

public class NetworkService
{
    private readonly DataBaseService _dbService;
    private readonly IPAddress _ipAddress = IPAddress.Parse("127.0.0.1");
    private readonly int _port = 8000;
    private readonly TcpListener _server;
    private readonly object _lock = new object();
    
    private static readonly Lazy<NetworkService> _instance = 
        new Lazy<NetworkService>(() => new NetworkService());
    
    public static NetworkService Instance => _instance.Value;

    private NetworkService()
    {
        _server = new TcpListener(_ipAddress, _port);
    }

    public async Task StartAsync()
    {
        _server.Start();
        Console.WriteLine($"Сервер запущен на {_ipAddress}:{_port}");

        try
        {
            while (true)
            {
                var client = await _server.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            _server.Stop();
            Console.WriteLine("Сервер остановился");
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        Console.WriteLine("Клиент подключился");
        try
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024 * 1024];
                var requestBuilder = new StringBuilder();

                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if  (bytesRead == 0) break;
                    
                    string requestPart = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    requestBuilder.Append(requestPart);

                    if (requestPart.EndsWith("\n"))
                    {
                        string fullRequest = requestBuilder.ToString().Trim();
                        Console.WriteLine($"Получено: {fullRequest}");
                        
                        string response = await ProcessRequestAsync(fullRequest);
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response + "\n");
                        
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        requestBuilder.Clear();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<string> ProcessRequestAsync(string request)
    {
        try
        {
            var parts = request.Split(' ', 2);
            string command = parts[0];
            string jsonData = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case "GET_USERS":
                    var users = await _dbService.GetAllUsersAsync();
                    return JsonSerializer.Serialize(users);
                case "ADD_USER":
                    var user = JsonSerializer.Deserialize<User>(jsonData);
                    await _dbService.AddUserAsync(user);
                    return "OK";
                default:
                    return "ERROR: Unknown command";
            }
        }
        catch (Exception e)
        {
            return $"ERROR: {e.Message}";
        }
    }
}