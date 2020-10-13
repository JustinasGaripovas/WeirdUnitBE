using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.GameLogic;
using System.Collections;
using WeirdUnitBE.GameLogic.TowerPackage;
using WeirdUnitBE.GameLogic.Services.Implementation;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketServerManager _manager;

        private ConcurrentDictionary<Room, GameState> roomDict = new ConcurrentDictionary<Room, GameState>();

        private ConcurrentDictionary<WebSocket, Room> socketToRoomDict = new ConcurrentDictionary<WebSocket, Room>();

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
                
                string enemyConnectionId = GetConnectionIdFromLobby();
                string currentConnectionId = _manager.AddSocket(webSocket); 
                
                await handleGameStart(enemyConnectionId, currentConnectionId, webSocket);

                //Room currentRoom = WaitForEnemyConnection(currentConnectionId); // BBZ

                Room currentRoom = null;
                
                await ReceiveMessage(webSocket, async(result, buffer) =>{
                    if(result.MessageType == WebSocketMessageType.Text)
                    {
                        if(currentRoom == null)
                        {
                            currentRoom = roomDict.Keys.Where(r => r.connID1 == currentConnectionId || r.connID2 == currentConnectionId).First();
                            System.Console.WriteLine("LOOOP ->" + currentRoom.roomID);
                        }
                        Console.WriteLine("Message Received");
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        //HandleMessage(message);
                        var newObject = JsonConvert.DeserializeObject<dynamic>(message);
                        /* TEST */
                        string command = newObject.command;
                        HandleJsonMessage(newObject);

                        /* TEST */

                        //Console.WriteLine("From room :" + currentRoom.roomID); // BBZ

                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                        return;
                    }
                    else if(result.MessageType == WebSocketMessageType.Close) // If socket-closed message
                    {       
                        string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key; // get sockets connID
                        Console.WriteLine("Received Close Message from " + id);
                        
                        /*
                        Room roomToClose = socketToRoomDict.TryGetValue(removedSocket);
                        socketToRoomDict.TryRemove(roomToClose);

                        WebSocket currentWebSocket = _manager.GetAllSockets().TryGetValue(roomToClose.connID1);
                        WebSocket enemyWebSocket = _manager.GetAllSockets().TryGetValue(roomToClose.connID2);
                        socketToRoomDict.TryRemove(currentWebSocket);
                        socketToRoomDict.TryRemove(enemyWebSocket);
                        */

                        _manager.GetAllSockets().TryRemove(id, out WebSocket removedSocket); // Remove socket from _sockets 
                        _manager._lobbySockets.TryRemove(id, out WebSocket removedSocket3); // Remove socket from _lobbysockets
                        

                        if(removedSocket.State == WebSocketState.Closed) // If socket is already closed - return;
                        {
                            return;
                        }
                        await removedSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None); // close the socket

                        Room tempRoom = (Room)roomDict.Keys.Where(room => id == room.connID1 || id == room.connID2).FirstOrDefault(); // get the room which has this connID
                        if(tempRoom == null) // If room doesn't exist -> return
                        {
                            return;
                        }
                        Console.WriteLine("Room:" + tempRoom.roomID + " id1=" + tempRoom.connID1 + "  id2="+tempRoom.connID2);
                        string id_2 = (id == tempRoom.connID1) ? tempRoom.connID2 : tempRoom.connID1; // get 2nd player's connID in the room 

                        WebSocket removedSocket2 = _manager.GetAllSockets().FirstOrDefault(s => s.Key == id_2).Value; // get 2nd player's socket
                        await removedSocket2.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None); // close 2nd player's socket

                        roomDict.TryRemove(tempRoom, out GameState removedGameState); // remove room from dictionary
                        Console.WriteLine("Room removed => " + removedGameState._room.roomID);

                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private Room WaitForEnemyConnection(string connID)
        {
            Room room;
            while((room = roomDict.Keys.Where(r => r.connID1 == connID || r.connID2 == connID).FirstOrDefault()) == null);
            //Console.WriteLine(room.roomID);
            return room;
        }

        private async Task handleGameStart(string enemyConnectionId, string currentConnectionId, WebSocket currentWebsocket)
        {
            if (!IsLobbyEmpty(enemyConnectionId))
            {
                var roomId = GenerateRoom(enemyConnectionId, currentConnectionId, out var room);
                var gameState = GenerateGameState(room);

                roomDict.TryAdd(room, gameState);

                var buffer = GetInitialGameSateCommandBuffer(roomId, gameState);
                var currentIDBuffer = GetConnIDCommandBuffer(currentConnectionId);

                await currentWebsocket.SendAsync(currentIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                await currentWebsocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                var enemySocket = GetEnemySocket(enemyConnectionId);
                var enemyIDBuffer = GetConnIDCommandBuffer(enemyConnectionId);

                // Refactor this 
                socketToRoomDict.TryAdd(enemySocket, room);
                socketToRoomDict.TryAdd(currentWebsocket, room);
                //----//

                await enemySocket.SendAsync(enemyIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                await enemySocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                ClearLobbySocket(enemyConnectionId, currentConnectionId);
            }
        }

        private static byte[] GetConnIDCommandBuffer(string connID)
        {
            var connIDInfo = new { command = "s:ConnID", payload = connID };

            var messageJson = JsonConvert.SerializeObject(connIDInfo, Formatting.Indented);
            return Encoding.UTF8.GetBytes(messageJson);

        }

        private WebSocket GetEnemySocket(string enemyConnectionId)
        {
            _manager._lobbySockets.TryGetValue(enemyConnectionId, out var enemySocket); // get Enemy web socket
            return enemySocket;
        }

        private void ClearLobbySocket(string enemyConnectionId, string currentConnectionId)
        {
            _manager._lobbySockets.TryRemove(currentConnectionId, out WebSocket removed1);
            _manager._lobbySockets.TryRemove(enemyConnectionId, out WebSocket removed2);
        }

        private static byte[] GetInitialGameSateCommandBuffer(string roomId, GameState gameState)
        {
            var gameStateInfo = GenerateInitialGamestateCommand(roomId, gameState);

            var messageJson = JsonConvert.SerializeObject(gameStateInfo, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(messageJson);
            return buffer;
        }

        private static object GenerateInitialGamestateCommand(string roomId, GameState gameState)
        {
            var gameStateInfo = new
            {
                command = "s:Initial", payload = new
                {
                    roomId = roomId, mapX = gameState.Get_MAP_DIMENSIONS().X, mapY = gameState.Get_MAP_DIMENSIONS().Y,
                    allTowers = gameState.getAllTowerList(), allPowerUps = gameState.GetAllPowerUps()
                }
            };
            return gameStateInfo;
        }

        private static bool IsLobbyEmpty(string enemyConnectionId)
        {
            return enemyConnectionId == String.Empty;
        }

        private static GameState GenerateGameState(Room room)
        {
            GameState gameState = new GameState(room);
            gameState.GenerateRandomGameState();
            return gameState;
        }

        private string GenerateRoom(string enemyConnectionId, string currentConnectionId, out Room room)
        {
            string roomId = GenerateRoomUUID();
            room = new Room(currentConnectionId, enemyConnectionId, roomId);
            return roomId;
        }

        private string GetConnectionIdFromLobby()
        {
            string connId2 = String.Empty;
            
            if (_manager._lobbySockets.Count > 0)
            {
                connId2 = _manager._lobbySockets.FirstOrDefault().Key;
                Console.WriteLine(connId2);
            }

            return connId2;
        }

        public void HandleJsonMessage(Object jsonObject)
        {
            //EventDispatcher.dispatchMessage(jsonObject.command);
            /*
                if command == "attack"
                jsonOb = {from(Tower), to(Tower)}

                if(jsonOb.'from'.unitCount/2 > jsonOb.'to')
                {
                    change gameState;
                    broadcast message('xarasho');
                }
                else
                {
                    broadcast message('ne xarasho, neuzkariausi towerio senelyzai')
                }
            */
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

        private string GenerateRoomUUID()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");
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