using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware.Observable.ConcreteObservers;

namespace WeirdUnitBE.Middleware.Observable
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        Task Broadcast(object data);
    }
}