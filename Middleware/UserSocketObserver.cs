using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware
{
    public class UserSocketObserver : IObserver
    {
        public WebSocket socket;
        public UserSocketObserver(WebSocket _socket)
        {
            socket = _socket;
        }

        public async Task SendData(ISubject subject, object data)
        {
            if(subject is RoomSubject roomSubject)
            {
                await socket.SendAsync((byte[])data, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}