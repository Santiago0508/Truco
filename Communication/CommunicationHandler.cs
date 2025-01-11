using System.Net.Sockets;

namespace Communication;

public class CommunicationHandler(TcpClient client)
{
    public void SendEvent(ServerSideEvents outgoingEvent)
    {
        byte[] data = [(byte)outgoingEvent];
        Send(data);        
    }
    
    public void SendEvent(ClientSideEvents outgoingEvent)
    {
        byte[] data = [(byte)outgoingEvent];
        Send(data);        
    }

    public void Send(byte[] data)
    {
        int offset = 0;
        int length = data.Length;

        var networkStream = client.GetStream();
        networkStream.Write(data, offset, length);
    }
    
    public byte[] ReceiveEvent()
    {
        var data = Receive(1);
        return data;
    }
    
    public byte[] Receive(int length)
    {
        byte[] data = new byte[length];
        int offset = 0;

        var networkStream = client.GetStream();

        while (offset < length)
        {
            int received = networkStream.Read(data, offset, length - offset);
            if (received == 0)
            {
                throw new SocketException();
            }

            offset += received;
        }

        return data;
    }

    public enum ServerSideEvents
    {
        Ack,
        Connected,
        Disconnected,
        UserTurn,
        OpponentTurn,
        DrewHand,
        OpponentAccepted,
        OpponentRejected,
        OpponentTruco,
        OpponentEnvido,
        OpponentFlor,
        OpponentPlayedCard,
        OpponentValue,
        OpponentForfeitHand,
        OpponentForfeitGame
    }
    
    public enum ClientSideEvents
    {
        Ack,
        PlayCard,
        Accept,
        Reject,
        Truco,
        Envido,
        Flor,
        ForfeitHand,
        ForfeitGame
    }
}