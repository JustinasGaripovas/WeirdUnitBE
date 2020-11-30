using System;
using System.Linq;
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

        public string GetConnectionIdFromLobby()
        {
            string connID = String.Empty;

            if (_lobbySockets.Count > 0)
            {
                connID = _lobbySockets.FirstOrDefault().Key;
            }
            else
            {
                throw new EnemyNotFoundException("Still waiting for another player to join the room...");
            }

            return connID;
        }

        public WebSocket GetSocketFromSocketPool(string connId)
        {
            WebSocket webSocket = _sockets[connId];
            return webSocket;
        }

        public string AddSocketToSocketPool(WebSocket socket)
        {
            string connId = Guid.NewGuid().ToString();
            _sockets.TryAdd(connId, socket);

            Console.WriteLine("Connection Added: " + connId);
            return connId;
        }

        public void AddClientToLobbyPool(string connId)
        {
            WebSocket webSocket = GetSocketFromSocketPool(connId);
            _lobbySockets.TryAdd(connId, webSocket);
        }

        public void RemoveSocketFromAllPools(string connId)
        {
            RemoveSocketFromLobbyPool(connId);
            RemoveSocketFromSocketPool(connId);
        }

        public void RemoveSocketsFromLobbyPool(params object[] connIds)
        {
            foreach(object connId in connIds)
            {
                RemoveSocketFromLobbyPool((string)connId);
            }
        }

        private void RemoveSocketFromLobbyPool(string connId)
        {
            _lobbySockets.TryRemove(connId, out _);
        }

        private void RemoveSocketFromSocketPool(string connId)
        {
            _sockets.TryRemove(connId, out _);
        }
    }
}