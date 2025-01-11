using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using Communication;

namespace Client;

class Program
{
    private static TcpClient _tcpClient;
    private static bool _running = true;
    private static string _localIp = "0.0.0.0";
    private static int _localPort = 0;
    private static string _remoteIp = "0.0.0.0";
    private static int _remotePort = 50000;


    static void Main(string[] args)
    {
        Console.Clear();
        _tcpClient = CreateTcpClient();
        while (_running)
        {
            try
            {
                var gameFlow = new GameFlow(_tcpClient);
                gameFlow.Run();
            }
            catch (Exception)
            {
                // Handle exception
            }
        }
    }

    private static TcpClient CreateTcpClient()
    {
        var localEndpoint = new IPEndPoint(IPAddress.Parse(_localIp), _localPort);
        var remoteEndpoint = new IPEndPoint(IPAddress.Parse(_remoteIp), _remotePort);
        var tcpClient = new TcpClient(localEndpoint);
        tcpClient.Connect(remoteEndpoint);
        return tcpClient;
    }
}