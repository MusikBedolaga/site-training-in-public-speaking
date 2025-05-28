using System.Net.Sockets;
using System.Text;

namespace TraniningWebsiteFront;

public class NetworkClient
{
    private readonly string _serverIp;
    private readonly int _serverPort;

    public NetworkClient(string ip, int port)
    {
        _serverIp = ip;
        _serverPort = port;
    }

    public async Task<string> SendCommandAsync(string command, string jsonData = null)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(_serverIp, _serverPort);
        
            using var stream = client.GetStream();
            string message = jsonData != null ? $"{command} {jsonData}" : command;
            byte[] requestBytes = Encoding.UTF8.GetBytes(message + "\n");

            await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке команды: {ex.Message}");
            return $"ERROR: {ex.Message}";
        }
    }
}