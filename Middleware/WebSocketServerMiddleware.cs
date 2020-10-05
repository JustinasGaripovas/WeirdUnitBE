using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.GameLogic;
using System.Collections;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketServerManager _manager;

        private bool isGameStateInitialized = false;

        private GameState gameState;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerManager manager)
        {
            gameState = GameState.GetInstance();
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
                var connectionInfo = new {command = "ConnID", payload=connId };
                var jsonMessage = JsonConvert.SerializeObject(connectionInfo, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);

                // Trying to send GameState info
                if(!isGameStateInitialized && _manager.GetAllSockets().Count == 1)
                {
                   gameState.GenerateRandomGameState(); 
                   isGameStateInitialized = true;
                }
                
                List<Tower> allTowers = gameState.GetAllTowers().Values.ToList<Tower>();
                var gameStateInfo = new {command = "initial", payload = new{ mapX = gameState.Get_MAP_DIMENSIONS().X, mapY = gameState.Get_MAP_DIMENSIONS().Y, _allTowers = allTowers , 
                    _allPowerUps = gameState.GetAllPowerUps()}};
                var anotherJsonMessage = JsonConvert.SerializeObject(gameStateInfo, Formatting.Indented);
                var buffer2 = Encoding.UTF8.GetBytes(anotherJsonMessage);
                
                await webSocket.SendAsync(buffer2, WebSocketMessageType.Text, true, CancellationToken.None);
                
                //await SendJSONAsync(webSocket);
               
                await ReceiveMessage(webSocket, async(result, buffer) =>{
                    if(result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine("Message Received");
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        //HandleMessage(message);
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

        public void HandleMessage(string messageJson)
        {
            // Check if message is in JSON format
            if(IsValidJson(messageJson))
            {
                Console.WriteLine("Json message is valid");
            }
        }

        private bool IsValidJson(string messageJson)
        {
            if(string.IsNullOrWhiteSpace(messageJson))
            {
                Console.WriteLine("null or whitespace");
                return false;
            }
            messageJson = messageJson.Trim();
            
            if((messageJson.StartsWith("{") && messageJson.EndsWith("}")) || // For object
                (messageJson.StartsWith("[") && messageJson.EndsWith("]"))) // For array
            {
                try
                {
                    var obj = JToken.Parse(messageJson);
                    return true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
            return false;
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