using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
//using Newtonsoft.Json;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketServerManager _manager;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync( );
                    Console.WriteLine("WebSocket Connected");

                    string connId = _manager.AddSocket(webSocket);
                    await SendConnIDASync(webSocket, connId);

                    await ReceiveMessage(webSocket, async(result, buffer) =>{
                        if(result.MessageType == WebSocketMessageType.Text)
                        {
                            Console.WriteLine("Message Received");
                            Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                            return;
                        }
                        else if(result.MessageType == WebSocketMessageType.Close)
                        {       
                            string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key;
                            Console.WriteLine("Received Close Message from " + id);
                            _manager.GetAllSockets().TryRemove(id, out WebSocket removedSocket);
                            await removedSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                            return;
                        }
                    });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task SendConnIDASync(WebSocket socket, string connId)
        {
            var buffer = Encoding.UTF8.GetBytes("ConnID: " + connId);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);           
        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while(socket.State == WebSocketState.Open){
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
            }
        }
        
    }
}