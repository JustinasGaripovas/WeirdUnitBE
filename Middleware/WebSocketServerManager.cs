using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WeirdUnitBE.Middleware
{
    public class WebSocketServerManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string, WebSocket> _lobbySockets = new ConcurrentDictionary<string, WebSocket>();

        public ConcurrentDictionary<string, WebSocket> GetAllSockets()
        {
            return _sockets;
        }

        public string AddSocket(WebSocket socket)
        {
            string connId = Guid.NewGuid().ToString();

            _sockets.TryAdd(connId, socket);
            AddLobbySocket(connId, socket);

            Console.WriteLine("Connection Added: " + connId);
            return connId;
        }

        private void AddLobbySocket(string connId, WebSocket socket)
        {
            _lobbySockets.TryAdd(connId, socket);
        }
    }
}