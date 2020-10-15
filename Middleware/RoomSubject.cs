using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware
{
    public class RoomSubject : MarshalByRefObject, ISubject
    {
        private List<Object> clients = new List<Object>();

        public RoomSubject(params object[] _clients)
        {
            foreach(object client in _clients)
            {
                Attach((IObserver)client);
            }
        }
        
        public void Attach(IObserver client)
        {
            Console.WriteLine("observer attached");
            clients.Add(client);
        }

        public async Task Broadcast(object data)
        {
            for(int i =0; i<clients.Count; i++)
            {
                await ((IObserver) clients[i]).SendData(this, data);
            }
        }
    }
}