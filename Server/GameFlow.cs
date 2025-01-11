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
        var handData = new byte[3];
        handData[0] = (byte)hand[0].Id;
        handData[1] = (byte)hand[1].Id;
        handData[2] = (byte)hand[2].Id;
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