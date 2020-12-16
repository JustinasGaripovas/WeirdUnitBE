using System;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.Services.Interfaces;


namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class ConcreteMemento : IMemento
    {      
        private Position _positionState;
        private string _towerTypeState;

        public ConcreteMemento(Position position, string towerType)
        {
            this._positionState = position;
            this._towerTypeState = towerType;
        }

        public Position GetPositionState()
        {
            return this._positionState;
        }

        public string GetTypeState()
        {
            return this._towerTypeState;
        }        
    }

}