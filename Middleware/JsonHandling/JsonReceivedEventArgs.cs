using System;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonReceivedEventArgs : EventArgs
    {
        public string roomId { get; set; }
        public dynamic jsonObj { get; set; }

        public JsonReceivedEventArgs(string _roomId, dynamic _jsonObj)
        {
            roomId = _roomId;
            jsonObj = _jsonObj;
        }
    }
}