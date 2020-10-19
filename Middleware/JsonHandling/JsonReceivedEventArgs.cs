using System;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonReceivedEventArgs : EventArgs
    {
        public Room room { get; set; }
        public dynamic jsonObj { get; set; }

        public JsonReceivedEventArgs(Room _room, dynamic _jsonObj)
        {
            room = _room;
            jsonObj = _jsonObj;
        }
    }
}