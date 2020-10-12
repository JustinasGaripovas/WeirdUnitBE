using System;

namespace WeirdUnitBE.Middleware
{
    public class UserSocket : IObserver
    {
        public string socketName {get;set;}
        public UserSocket(string name)
        {
            socketName = name;
        }

        public void Update(ISubject subject)
        {
            if(subject is RoomManager roomManager)
            {
                Console.WriteLine(String.Format("{0} reporting message {1} ", socketName, roomManager.message));
            }
        }
    }
}