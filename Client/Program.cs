using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;

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
        Console.WriteLine("Connected to the server");
        while (_running)
        {
            // Do your thing
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