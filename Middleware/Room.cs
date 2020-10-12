using WeirdUnitBE.GameLogic.Services.Implementation;

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

        public bool ContainsUsers(string _connID1, string _connID2)
        {
            if(connID1 == _connID1 && connID2 == _connID2)
            {
                return true;
            }
            else if(connID1 == _connID2 && connID2 == _connID1)
            {
                return true;
            }
            return false;
        }
    }
}