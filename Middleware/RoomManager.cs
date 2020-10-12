using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;

namespace WeirdUnitBE.Middleware
{
    public class RoomManager : ISubject
    {
        private List<IObserver> _observers;

        private string _message;
        public string message
        {
            get { return _message; }
            set
            {
                _message = value;
                Notify();
            }
        }
        public RoomManager()
        {
            _observers = new List<IObserver>();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Notify()
        {
            _observers.ForEach(o =>
            {
                o.Update(this);
            });
        }
    }
}