using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WeirdUnitBE.GameLogic;

namespace WeirdUnitBE.Middleware.Observable.ConcreteSubjects
{
    public class RoomSubject : MarshalByRefObject, ISubject
    {
        private List<Object> clients;
        public GameState gameState { get; set; }

        public RoomSubject(params object[] _clients)
        {
            clients = new List<Object>();
            foreach (object client in _clients)
            {
                Attach((IObserver)client);
            }
        }

        public RoomSubject(GameState gameState, params object[] _clients)
        {
            clients = new List<Object>();
            foreach (object client in _clients)
            {
                Attach((IObserver)client);
            }

            this.gameState = gameState;
        }

        public void Attach(IObserver client)
        {
            Console.WriteLine("observer attached");
            clients.Add(client);
        }

        public async Task Broadcast(object data)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                await ((IObserver)clients[i]).SendData(this, data);
            }
        }

    }
}