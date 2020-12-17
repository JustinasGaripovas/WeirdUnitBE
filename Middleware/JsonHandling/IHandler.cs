using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        Task Handle(Room room, dynamic jsonObj);
    }
}