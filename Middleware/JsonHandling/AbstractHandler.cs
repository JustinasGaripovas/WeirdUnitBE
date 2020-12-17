using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;
        
        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;

            return handler;
        }
        public async virtual Task Handle(Room room, dynamic jsonObj)
        {
            if(this._nextHandler != null)
            {
                await this._nextHandler.Handle(room, jsonObj);
            }           
        }
    }
}