using System;
//using System.Object;
using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        Task Broadcast(object data);
    }
}