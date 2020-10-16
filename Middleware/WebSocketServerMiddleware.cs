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

        private ConcurrentDictionary<WebSocket, Room> socketToRoomDict = new ConcurrentDictionary<WebSocket, Room>();

        private ConcurrentDictionary<string, GameState> roomIdToGamestateDict = new ConcurrentDictionary<string, GameState>();

        private ConcurrentDictionary<string, RoomSubject> roomIdToRoomsubjectDict = new ConcurrentDictionary<string, RoomSubject>();

        private ConcurrentDictionary<string, JsonMessageHandler> roomIdToJsonHandler = new ConcurrentDictionary<string, JsonMessageHandler>();

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

                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count); // Extract the message

                        Console.WriteLine("Message Received");
                        Console.WriteLine($"Message: {message}");
                        
                        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(message); // Convert message string to json object
                        Room currentRoom = socketToRoomDict[webSocket]; // Get the current room

                        JsonMessageHandler jsonMessageHandler = new JsonMessageHandler();

                        jsonMessageHandler.OnMoveToEvent += HandleOnMoveToEvent;

                        await jsonMessageHandler.HandleJsonMessage(currentRoom.roomID, jsonObj);
  
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close) // If socket-closed message
                    {
                        Console.WriteLine("Received Close Message from " + currentConnectionId);

                        _manager.GetAllSockets().TryRemove(currentConnectionId, out _); // Remove socket from _sockets 
                        _manager._lobbySockets.TryRemove(currentConnectionId, out _); // Remove socket from _lobbysockets


                        if (webSocket.State == WebSocketState.Closed) { return; } // If socket is already closed -> return;

                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None); // close the socket

                        if (!socketToRoomDict.ContainsKey(webSocket)) { return; } // If room isn't created yet -> return

                        WebSocket enemySocket = socketToRoomDict[webSocket].enemyWebSocket; // get enemy socket
                        await enemySocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None); // close enemy socket

                        // Whipe all info about the room
                        socketToRoomDict.TryRemove(webSocket, out Room removedRoom);
                        socketToRoomDict.TryRemove(enemySocket, out _);
                        roomIdToGamestateDict.TryRemove(removedRoom.roomID, out _);
                        roomIdToRoomsubjectDict.TryRemove(removedRoom.roomID, out _);

                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task handleGameStart(string enemyConnectionId, string currentConnectionId, WebSocket currentWebsocket)
        {
            if (!IsLobbyEmpty(enemyConnectionId))
            {
                var gameState = GenerateGameState(currentConnectionId, enemyConnectionId);

                var currentIDBuffer = GetConnIDCommandBuffer(currentConnectionId);
                await currentWebsocket.SendAsync(currentIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None); // send currentId to current user

                var enemySocket = GetEnemySocket(enemyConnectionId);
                var enemyIDBuffer = GetConnIDCommandBuffer(enemyConnectionId);
                await enemySocket.SendAsync(enemyIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None); // Send enemyId to enemy

                // Refactor this 
                var roomId = GenerateRoomUUID();
                socketToRoomDict.TryAdd(currentWebsocket, new Room(currentConnectionId, enemyConnectionId, roomId, enemySocket));
                socketToRoomDict.TryAdd(enemySocket, new Room(enemyConnectionId, currentConnectionId, roomId, currentWebsocket));
                roomIdToGamestateDict.TryAdd(roomId, gameState);              
                
                RoomSubject roomSubject = new RoomSubject
                (
                    new UserSocketObserver(currentWebsocket),
                    new UserSocketObserver(enemySocket)
                );
                
                roomIdToRoomsubjectDict.TryAdd(roomId, roomSubject);

                var buffer = GetInitialGameSateCommandBuffer(roomId, gameState);
                await roomSubject.Broadcast(buffer);
                
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
            WebSocket enemySocket = _manager._lobbySockets[enemyConnectionId]; // get Enemy web socket
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
                command = "s:Initial",
                payload = new
                {
                    roomId = roomId,
                    mapX = gameState.Get_MAP_DIMENSIONS().X,
                    mapY = gameState.Get_MAP_DIMENSIONS().Y,
                    allTowers = gameState.getAllTowerList(),
                    allPowerUps = gameState.GetAllPowerUps()
                }
            };
            return gameStateInfo;
        }

        private static bool IsLobbyEmpty(string enemyConnectionId)
        {
            return enemyConnectionId == String.Empty;
        }

        private static GameState GenerateGameState(string user1, string user2)
        {
            GameState gameState = new GameState();
            gameState.GenerateRandomGameState(user1, user2);
            return gameState;
        }

        private string GetConnectionIdFromLobby()
        {
            string connID = String.Empty;

            if (_manager._lobbySockets.Count > 0)
            {
                connID = _manager._lobbySockets.FirstOrDefault().Key;
            }

            return connID;
        }

        private async void HandleOnMoveToEvent(object sender, JsonReceivedEventArgs args)
        {
            Console.WriteLine("Handling MoveTo NOW!");
            dynamic jsonObj = args.jsonObj;
            string roomId = args.roomId;
            GameState gameState = roomIdToGamestateDict[roomId];
 
            var payload = jsonObj.payload;

            Position positionFrom = new Position((int)payload.moveFrom.X, (int)payload.moveFrom.Y);
            Position positionTo = new Position((int)payload.moveTo.X, (int)payload.moveTo.Y);

            Tower towerFrom = gameState.positionToTowerDict[positionFrom];
            Tower towerTo = gameState.positionToTowerDict[positionTo];

            // VALIDATION
            if (positionFrom == positionTo)
            {
                System.Console.WriteLine("Move is invalid");
            }
            else if (towerFrom.owner == towerTo.owner)
            {
                // Send friendly recruits                   
                System.Console.WriteLine("Sending friendly units...");
                    
                roomIdToGamestateDict[roomId].ReinforceFriendly(positionFrom, positionTo, out var affectedTowers);
                var gameStateInfo = new
                {
                    command = "s:MoveTo",
                    payload = new { allTowers = affectedTowers }
                };
                // broadcast changes
                var messageJson = JsonConvert.SerializeObject(gameStateInfo, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(messageJson);

                await roomIdToRoomsubjectDict[roomId].Broadcast(buffer);
            }
            else if (true)
            {
                System.Console.WriteLine("Sending units...");
                // change gamestate
                roomIdToGamestateDict[roomId].PerformAttack(positionFrom, positionTo, out var affectedTowers);
                var gameStateInfo = new
                {
                    command = "s:MoveTo",
                    payload = new { allTowers = affectedTowers }
                };
                // broadcast changes
                var messageJson = JsonConvert.SerializeObject(gameStateInfo, Formatting.Indented);
                var buffer = Encoding.UTF8.GetBytes(messageJson);

                await roomIdToRoomsubjectDict[roomId].Broadcast(buffer);
            }   
        }

        private string GenerateRoomUUID()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");
        }

        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
            }
        }

    }
}