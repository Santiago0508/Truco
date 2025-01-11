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
        var hand = _communicationHandler.Receive(3);
        SendAck();
        var card1 = DecodeCard(hand[0]);
        var card2 = DecodeCard(hand[1]);
        var card3 = DecodeCard(hand[2]);
        Console.WriteLine("Mano:");
        Console.WriteLine('\t' + card1);
        Console.WriteLine('\t' + card2);
        Console.WriteLine('\t' + card3);
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