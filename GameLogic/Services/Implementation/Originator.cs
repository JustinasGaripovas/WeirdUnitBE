using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using WeirdUnitBE.GameLogic.Services.Interfaces;
using WeirdUnitBE.GameLogic;

namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class Originator
    {      
        private Position _positionState;
        private string _towerTypeState;

        public Originator()
        {
        }       

        public void ChangeState(Position position, string towerType)
        {
            this._positionState = position;
            this._towerTypeState = towerType;
        }

        public IMemento Save()
        {
            return new ConcreteMemento(this._positionState, this._towerTypeState);
        }

        /*
        public void Restore(IMemento memento)
        {
            if(!(memento is ConcreteMemento))
            {
                throw new Exception("Uknown memento class " + memento.ToString());
            }

            this._state = memento.GetState();
            Console.Write($"Originator: My state has changed to: {_state}");
        }
        */
    }

}