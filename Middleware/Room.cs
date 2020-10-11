
namespace WeirdUnitBE.Middleware
{
    public class Room
    {
        public string connID1, connID2, roomID;
        public Room(string _connID1, string _connID2, string _roomID)
        {
            connID1 = _connID1;
            connID2 = _connID2;
            roomID = _roomID;
        }
    }
}