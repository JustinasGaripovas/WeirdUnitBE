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
                ConsoleLogger.LogToConsole("WebSocket Connected");

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();               
                string currentConnectionId = _manager.AddSocketToSocketPool(webSocket);
                await HandleGameStart(currentConnectionId); 
                _manager.AddSocketToLobbyPool(webSocket, currentConnectionId);                              
                
                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string stringMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        ConsoleLogger.LogToConsole("Message Received");
                        ConsoleLogger.LogToConsole($"Message: {stringMessage}");

                        string stringJson = ConvertToJsonString(stringMessage);                
                        
                        Room currentRoom = socketToRoomDict[webSocket];
                        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(stringJson);
                        await jsonHandler.HandleJsonMessage(currentRoom, jsonObj);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        ConsoleLogger.LogToConsole("Received Close Message from " + currentConnectionId);

                        _manager.RemoveSocketFromAllPools(currentConnectionId);

                        if(!WebSocketIsClosed(webSocket))
                        {
                            await CloseWebSocket(webSocket, result);

                            if(WebSocketBelongsToARoom(webSocket))
                            {
                                WebSocket enemySocket = GetEnemyWebSocket(webSocket);
                                await CloseWebSocket(enemySocket, result);
                                DeleteRoom(webSocket, enemySocket);
                            }
                        }                      
                    }
                    return;
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleGameStart(string currentConnectionId)
        {
            try
            {
                string enemyConnectionId = FindEnemyConnectionId();
                await InitializeGameStart(currentConnectionId, enemyConnectionId);
            }
            catch(Exception e)
            {
                ConsoleLogger.LogToConsole(e.Message);               
            }         
        }

        private string FindEnemyConnectionId()
        {
            string enemyConnectionId = _manager.GetConnectionIdFromLobby();    
            if(!EnemyFound(enemyConnectionId))
            {
                throw new EnemyNotFoundException("Still waiting for another player to join the room...");
            }

            return enemyConnectionId;
        }

        private static bool EnemyFound(string enemyConnectionId)
        {
            return enemyConnectionId != String.Empty;
        }

        public async Task InitializeGameStart(string currentConnectionId, string enemyConnectionId)
        {
            await SendConnectionIdToClient(currentConnectionId);
            await SendConnectionIdToClient(enemyConnectionId); 

            var gameState = GenerateGameState(currentConnectionId, enemyConnectionId);  
            WebSocket currentWebsocket = _manager.GetSocketFromSocketPool(currentConnectionId);  
            WebSocket enemySocket = _manager.GetSocketFromLobby(enemyConnectionId);            
            RoomSubject roomSubject = new RoomSubject
            (
                gameState,
                new UserSocketObserver(currentWebsocket),
                new UserSocketObserver(enemySocket)
            );

            _manager.RemoveSocketsFromLobbyPool(currentConnectionId, enemyConnectionId);
                     
            var roomId = Room.GenerateRoomUUID();

            socketToRoomDict.TryAdd(currentWebsocket, new Room(currentConnectionId, enemyConnectionId, roomId, enemySocket));
            socketToRoomDict.TryAdd(enemySocket, new Room(enemyConnectionId, currentConnectionId, roomId, currentWebsocket));
            roomIdToRoomsubjectDict.TryAdd(roomId, roomSubject);

            ConfigureEventHandler(roomSubject);

            await BroadcastInitialGameStateToRoom(gameState, roomId);
        }

        private async Task SendConnectionIdToClient(string connectionId)
        {
            JsonMessageFormatterTemplate connIdFormatter = new JsonConnectionIdMessageFormatter();

            var connectionIdBuffer = connIdFormatter.FormatJsonBufferFromParams(connectionId);
            await SendBufferToClient(connectionIdBuffer, connectionId);
        }

        private async Task SendBufferToClient(dynamic buffer, string clientId)
        {
            WebSocket socket = _manager.GetAllSockets()[clientId];
            await socket.SendAsync((byte[])buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        } 

        private GameState GenerateGameState(string client1, string client2)
        {
            GameStateBuilder gameStateBuilder = new GameStateBuilder();
            var gameStateDirector = new GameStateDirector(gameStateBuilder, client1, client2);
            GameState gameState = gameStateDirector.GetResult();

            return gameState;
        }

        private void ConfigureEventHandler(RoomSubject roomSubject)
        {
            jsonHandler = new JsonMessageHandler(roomSubject);
            jsonHandler.OnMoveToEvent += HandleOnMoveToEvent;
            jsonHandler.OnPowerUpEvent += HandleOnPowerUpEvent;
            jsonHandler.UpgradeTowerEvent += HandleUpgradeEvent;
            jsonHandler.OnArrivedToEvent += HandleOnArrivedToEvent;
        }

        private async Task BroadcastInitialGameStateToRoom(GameState gameState, string roomId)
        {
            JsonMessageFormatterTemplate formatter = new JsonInitialGameStateMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(roomId, gameState);

            await roomIdToRoomsubjectDict[roomId].Broadcast(buffer);
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

        private string ConvertToJsonString(string stringMessage)
        {
            Adapter adapter = new Adapter();
            string stringJson = adapter.ConvertToJson(stringMessage);

            return stringJson;
        }

        public bool WebSocketIsClosed(WebSocket webSocket)
        {
            return webSocket.State == WebSocketState.Closed;
        }

        public async Task CloseWebSocket(WebSocket webSocket, WebSocketReceiveResult receiveResult)
        {
            await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
        }

        public bool WebSocketBelongsToARoom(WebSocket webSocket)
        {
            return socketToRoomDict.ContainsKey(webSocket);
        }

        public WebSocket GetEnemyWebSocket(WebSocket webSocket)
        {
            WebSocket enemySocket = socketToRoomDict[webSocket].enemyWebSocket;
            return enemySocket;
        }

        public void DeleteRoom(WebSocket currentSocket, WebSocket enemySocket)
        {
            socketToRoomDict.TryRemove(currentSocket, out Room removedRoom);
            socketToRoomDict.TryRemove(enemySocket, out _);
            roomIdToRoomsubjectDict.TryRemove(removedRoom.roomID, out _);
        } 

        private async void HandleOnPowerUpEvent(object sender, JsonReceivedEventArgs args)
        {
            IGameStateExecutable executive = new PowerUpExecutive();
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
    }
}