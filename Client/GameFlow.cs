using System.Net.Sockets;
using Communication;

namespace Client;

public class GameFlow(TcpClient client)
{
    private readonly CommunicationHandler _communicationHandler = new(client);
    
    public void Run()
    {
        ReceiveConfirmation();
        AwaitTurn();
        DrawHand();
    }

    private void ReceiveConfirmation()
    {
        var incomingEvent = GetServerEvent();
        if (incomingEvent != CommunicationHandler.ServerSideEvents.Connected)
        {
            // Handle error
        }
        Console.WriteLine($"Server: {incomingEvent}");
        SendAck();
    }
    
    private void AwaitTurn()
    {
        var incomingEvent = GetServerEvent();
        if (incomingEvent != CommunicationHandler.ServerSideEvents.UserTurn)
        {
            // Handle error
        }
        Console.WriteLine($"Server: {incomingEvent}");
        SendAck();
    }

    

    private void DrawHand()
    {
        var incomingEvent = GetServerEvent();
        if (incomingEvent != CommunicationHandler.ServerSideEvents.UserTurn)
        {
            // Handle error
        }
        SendAck();
        
        var handData = _communicationHandler.Receive(3);
        SendAck();
        
        DecodeHand(handData, out var muestra, out var card1, out var card2, out var card3);

        Console.WriteLine($"Muestra: {muestra}");
        Console.WriteLine("Mano:");
        Console.WriteLine("[1] " + card1);
        Console.WriteLine("[2] " + card2);
        Console.WriteLine("[3] " + card3);
    }

    private void DecodeHand(byte[] handData, out string muestra, out string card1, out string card2, out string card3)
    {
        var packedData = (handData[0] << 16) | (handData[1] << 8) | handData[2];
        muestra = DecodeCard((packedData >> 18) & 0x3F);
        card1 = DecodeCard((packedData >> 12) & 0x3F);
        card2 = DecodeCard((packedData >> 6) & 0x3F);
        card3 = DecodeCard(packedData & 0x3F);
    }

    private string DecodeCard(int id)
    {
        string suit = (id / 10) switch
        {
            0 => "Espada",
            1 => "Basto",
            2 => "Oro",
            3 => "Copa",
            _ => throw new ArgumentException("Invalid card ID")
        };

        var value = id % 10;
        value += value > 7 ? 3 : 1;

        return $"{value} de {suit}";
    }
    
    private void SendAck()
    {
        _communicationHandler.SendEvent(CommunicationHandler.ClientSideEvents.Ack);
    }
    
    private CommunicationHandler.ServerSideEvents GetServerEvent()
    {
        var data = _communicationHandler.ReceiveEvent();
        return (CommunicationHandler.ServerSideEvents)data[0];
    }
}