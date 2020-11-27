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
using WeirdUnitBE.Middleware.JsonHandling;
using WeirdUnitBE.Middleware.Observable.ConcreteObservers;
using WeirdUnitBE.Middleware.Observable.ConcreteSubjects;
using System.Collections;
using WeirdUnitBE.GameLogic.Services;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Services.Implementation;
using WeirdUnitBE.Middleware.XmlHandling;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketServerManager _manager;

        private ConcurrentDictionary<WebSocket, Room> socketToRoomDict = new ConcurrentDictionary<WebSocket, Room>();

        private ConcurrentDictionary<string, RoomSubject> roomIdToRoomsubjectDict = new ConcurrentDictionary<string, RoomSubject>();

        private JsonMessageHandler jsonHandler;
        
        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == Constants.RoutingConstants.WEBSOCKET_REQUEST_PATH && context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket Connected");

                string enemyConnectionId = GetConnectionIdFromLobby();
                string currentConnectionId = _manager.AddSocket(webSocket);

                await HandleGameStart(enemyConnectionId, currentConnectionId, webSocket);
                
                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        Console.WriteLine("Message Received");
                        Console.WriteLine($"Message: {message}");

                        Adapter adapter = new Adapter();
                        if(!adapter.IsJson(message))
                        {
                            message = adapter.ConvertToJson(message);
                        }

                        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(message);
                        Room currentRoom = socketToRoomDict[webSocket];

                        await jsonHandler.HandleJsonMessage(currentRoom, jsonObj);
                        
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Received Close Message from " + currentConnectionId);

                        _manager.GetAllSockets().TryRemove(currentConnectionId, out _);
                        _manager._lobbySockets.TryRemove(currentConnectionId, out _);

                        if (webSocket.State == WebSocketState.Closed) { return; }

                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                        if (!socketToRoomDict.ContainsKey(webSocket)) { return; }

                        WebSocket enemySocket = socketToRoomDict[webSocket].enemyWebSocket;
                        await enemySocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                        DeleteRoom(webSocket, enemySocket);
                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
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

        private async Task HandleGameStart(string enemyConnectionId, string currentConnectionId, WebSocket currentWebsocket)
        {
            if (!IsLobbyEmpty(enemyConnectionId))
            {
                var gameStateDirector = new GameStateDirector(new GameStateBuilder(), currentConnectionId, enemyConnectionId );
                var gameState = gameStateDirector.GetResult();

                var currentIDBuffer = GetConnIDCommandBuffer(currentConnectionId);
                await currentWebsocket.SendAsync(currentIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

                var enemySocket = GetEnemySocket(enemyConnectionId);
                var enemyIDBuffer = GetConnIDCommandBuffer(enemyConnectionId);
                await enemySocket.SendAsync(enemyIDBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
 
                var roomId = Room.GenerateRoomUUID();
                socketToRoomDict.TryAdd(currentWebsocket, new Room(currentConnectionId, enemyConnectionId, roomId, enemySocket));
                socketToRoomDict.TryAdd(enemySocket, new Room(enemyConnectionId, currentConnectionId, roomId, currentWebsocket));

                RoomSubject roomSubject = new RoomSubject
                (
                    gameState,
                    new UserSocketObserver(currentWebsocket),
                    new UserSocketObserver(enemySocket)
                );
                
                roomIdToRoomsubjectDict.TryAdd(roomId, roomSubject);

                ConfigureEventHandler(roomSubject);
                
                var buffer = GetInitialGameSateCommandBuffer(roomId, gameState);
                await roomSubject.Broadcast(buffer);

                ClearLobbySocket(enemyConnectionId, currentConnectionId);
            }
        }

        private static bool IsLobbyEmpty(string enemyConnectionId)
        {
            return enemyConnectionId == String.Empty;
        }

        private static byte[] GetConnIDCommandBuffer(string connID)
        {
            var connIDInfo = new { command = Constants.JsonCommands.ServerCommands.CONN_ID, payload = connID };

            var messageJson = JsonConvert.SerializeObject(connIDInfo, Formatting.Indented);
            return Encoding.UTF8.GetBytes(messageJson);
        }

        private WebSocket GetEnemySocket(string enemyConnectionId)
        {
            WebSocket enemySocket = _manager._lobbySockets[enemyConnectionId];
            return enemySocket;
        }

        private void ConfigureEventHandler(RoomSubject roomSubject)
        {
            jsonHandler = new JsonMessageHandler(roomSubject);
            jsonHandler.OnMoveToEvent += HandleOnMoveToEvent;
            jsonHandler.OnPowerUpEvent += HandleOnPowerUpEvent;
            jsonHandler.UpgradeTowerEvent += HandleUpgradeEvent;
            jsonHandler.OnArrivedToEvent += HandleOnArrivedToEvent;
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
                command = Constants.JsonCommands.ServerCommands.INITIAL,
                payload = new
                {
                    roomId = roomId,
                    mapX = gameState.Get_MAP_DIMENSIONS().X,
                    mapY = gameState.Get_MAP_DIMENSIONS().Y,
                    allTowers = gameState.GetAllTowers(),
                    allPowerUps = gameState.GetAllPowerUps()
                }
            };
            return gameStateInfo;
        }

        private void ClearLobbySocket(string enemyConnectionId, string currentConnectionId)
        {
            _manager._lobbySockets.TryRemove(currentConnectionId, out _);
            _manager._lobbySockets.TryRemove(enemyConnectionId, out _);
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

        public void DeleteRoom(WebSocket currentSocket, WebSocket enemySocket)
        {
            socketToRoomDict.TryRemove(currentSocket, out Room removedRoom);
            socketToRoomDict.TryRemove(enemySocket, out _);
            roomIdToRoomsubjectDict.TryRemove(removedRoom.roomID, out _);
        }     

        private async void HandleOnPowerUpEvent(object sender, JsonReceivedEventArgs args)
        {
            IGameStateExecutable executive = new ArrivedToExecutive();
            await ExecuteCommandAndNotifyClients(executive, args);
        }

        private async void HandleOnArrivedToEvent(object sender, JsonReceivedEventArgs args)
        {
            IGameStateExecutable executive = new ArrivedToExecutive();
            await ExecuteCommandAndNotifyClients(executive, args);
        }

        private async void HandleOnMoveToEvent(object sender, JsonReceivedEventArgs args)
        {
            IGameStateExecutable executive = new MoveToExecutive();   
            await ExecuteCommandAndNotifyClients(executive, args); 
        }

        private async void HandleUpgradeEvent(object sender, JsonReceivedEventArgs args)
        { 
            IGameStateExecutable executive = new UpgradeTowerExecutive();
            await ExecuteCommandAndNotifyClients(executive, args); 
        }

        private async Task ExecuteCommandAndNotifyClients(IGameStateExecutable executive, JsonReceivedEventArgs args)
        {
            try
            {
                string roomId = args.room.roomID;
                GameState gameState = roomIdToRoomsubjectDict[roomId].gameState;

                var buffer = executive.ExecuteCommand(args, gameState); // Can throw exception       
                await roomIdToRoomsubjectDict[roomId].Broadcast(buffer);
            }
            catch(InvalidUpgradeException e)
            {
                string clientId = args.room.currentID;

                await FormatExceptionBufferAndSendToClient(e, clientId);               
            } 
        }

        public async Task FormatExceptionBufferAndSendToClient(Exception e, string clientId)
        {
            JsonMessageFormatterTemplate formatter = new JsonErrorMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(e);

            await SendBufferToClient(buffer, clientId); 
        }

        private async Task SendBufferToClient(dynamic buffer, string clientId)
        {
            WebSocket socket = _manager.GetAllSockets()[clientId];
            await socket.SendAsync((byte[])buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }      
    }
}