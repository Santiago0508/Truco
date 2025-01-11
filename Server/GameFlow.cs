using System.Net.Sockets;
using Communication;
using Logic;

namespace Server;

public class GameFlow(TcpClient client)
{
    private readonly CommunicationHandler _communicationHandler = new(client);
    private readonly Gamestate _gamestate = new(20, 2);
    
    public void Run()
    {
        SendConfirmation();
        _gamestate.StartTurn();
        IndicateTurn();
        SendHand();
        Thread.Sleep(10000);
    }
    
    private void SendConfirmation()
    {
        _communicationHandler.SendEvent(CommunicationHandler.ServerSideEvents.Connected);
        ReceiveAck();
    }
    
    private void IndicateTurn()
    {
        _communicationHandler.SendEvent(CommunicationHandler.ServerSideEvents.UserTurn);
        ReceiveAck();
    }

    private void SendHand()
    {
        _communicationHandler.SendEvent(CommunicationHandler.ServerSideEvents.UserTurn);
        ReceiveAck();
        var hand = _gamestate.Players[0].Hand;
        var muestra = _gamestate.Muestra;
        var packedData = (muestra.Id << 18) | (hand[0].Id << 12) | (hand[1].Id << 6) | hand[2].Id;
        var handData = new byte[3];
        handData[0] = (byte)((packedData >> 16) & 0xFF);
        handData[1] = (byte)((packedData >> 8) & 0xFF);
        handData[2] = (byte)(packedData & 0xFF);
        _communicationHandler.Send(handData);
        ReceiveAck();
    }
    
    private void ReceiveAck()
    {
        var incomingEvent = GetClientEvent();
        if (incomingEvent != CommunicationHandler.ClientSideEvents.Ack)
        {
            // Handle error
        }
    }
    
    private CommunicationHandler.ClientSideEvents GetClientEvent()
    {
        var data = _communicationHandler.ReceiveEvent();
        return (CommunicationHandler.ClientSideEvents)data[0];
    }
}