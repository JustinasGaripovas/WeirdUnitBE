using System;
using System.Net.WebSockets;

namespace WeirdUnitBE.Middleware
{
    public class Room
    {
        public string currentID, enemyID, roomID;
        public WebSocket enemyWebSocket;

        public Room(string _currentID, string _enemyID, string _roomID, WebSocket _enemyWebSocket)
        {
            currentID = _currentID;
            enemyID = _enemyID;
            roomID = _roomID;
            enemyWebSocket = _enemyWebSocket;  
        }

        public static string GenerateRoomUUID()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");
        }
    }
}