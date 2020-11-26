using System;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonReceivedEventArgs : EventArgs
    {
        public Room room { get; set; }
        public dynamic jsonObj { get; set; }

        public JsonReceivedEventArgs(Room room, dynamic jsonObj)
        {
            this.room = room;
            this.jsonObj = jsonObj;
        }
    }
}