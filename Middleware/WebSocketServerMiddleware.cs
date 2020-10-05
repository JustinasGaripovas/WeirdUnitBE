using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.GameLogic;
using System.Collections;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketServerManager _manager;

        private GameState gameState = new GameState();

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket Connected");

                string connId = _manager.AddSocket(webSocket); 
                var connectionInfo = new {command = "ConnID", _connId=connId };
                var jsonMessage = JsonConvert.SerializeObject(connectionInfo, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                //await SendJSONAsync(webSocket);
               
                await ReceiveMessage(webSocket, async(result, buffer) =>{
                    if(result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine("Message Received");
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        //JObject o = JObject.Parse(message);

                        var newObject = JsonConvert.DeserializeObject<dynamic>(message);
                        Console.WriteLine("From :" + newObject.From);
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
        
        private async Task SendJSONAsync(WebSocket socket)
        {
            var jsonToWrite = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(jsonToWrite);
            System.Console.WriteLine(jsonToWrite);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private string GenerateRoomUUID()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=","");
            GuidString = GuidString.Replace("+","");

            return GuidString;
        }

        private async Task SendConnIDASync(WebSocket socket, string connId)
        {
            var jsontowriteid = JsonConvert.SerializeObject("ConnId: " + connId, Formatting.Indented);
            string command = "Command: connID";
            var jsonToWritecommand = JsonConvert.SerializeObject(command, Formatting.Indented);

            var buffer = Encoding.UTF8.GetBytes(jsonToWritecommand + jsontowriteid);

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