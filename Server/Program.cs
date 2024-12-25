using System.Net;
using System.Net.Sockets;
using Logic;

namespace Server;

public class Program
{
    private static bool _serverIsRunning = true;
    private static readonly TcpListener _tcpListener = new(IPAddress.Any, 50000); //TODO: Extract to config file
    private static readonly List<TcpClient> _clients = [];
    
    public static async Task Main(string[] args)
    {
        Console.Clear();
        _tcpListener.Start();
        _ = AcceptClientsAsync(_tcpListener);
        Console.WriteLine("Server started");
        _ = IOLoopAsync(_tcpListener);
    }
    
    private static async Task AcceptClientsAsync(TcpListener tcpListener)
    {
        while (_serverIsRunning)
        {
            try
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                _clients.Add(client);
                _ = HandleClientAsync(client);
            }
            catch (Exception)
            {
                _serverIsRunning = false;
                // Handle exception
            }
        }
    }
    
    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
            GameDirector gameDirector = new GameDirector(40, 2);
            while (_serverIsRunning)
            {
                //handle client
            }
        }
        catch (Exception)
        {
            // Handle exception
        }
        finally
        {
            Console.WriteLine("Client disconnected {client.Client.RemoteEndPoint}");
            _clients.Remove(client);
            client.Close();
        }
    }
    
    private static async Task IOLoopAsync(TcpListener tcpListener)
    {
        while (_serverIsRunning)
        {
            var command = Console.ReadLine()?.Trim().ToLower()!;
            switch (command)
            {
                case "shutdown" or "s":
                    tcpListener.Stop();
                    await CloseClientsAsync();
                    _serverIsRunning = false;
                    break;
                case "reboot" or "r":
                    Console.WriteLine("Server rebooting...");
                    tcpListener.Stop();
                    await CloseClientsAsync();
                    tcpListener.Start();
                    Console.WriteLine("Server rebooted");
                    break;
                case "users" or "u":
                    Console.WriteLine($"Connected users: {_clients.Count}");
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    Console.WriteLine("Valid commands: shutdown, reboot, users");
                    break;
            }
        }
    }
    
    private static async Task CloseClientsAsync()
    {
        foreach (var client in _clients)
        {
            client.Close();
        }
    }
}