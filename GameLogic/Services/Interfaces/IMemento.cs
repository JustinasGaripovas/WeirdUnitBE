using System;
using WeirdUnitBE.GameLogic;

namespace WeirdUnitBE.GameLogic.Services.Interfaces
{
    public interface IMemento
    {
        Position GetPositionState();
        string GetTypeState();
    }
}
