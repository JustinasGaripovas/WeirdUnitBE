using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic.Services.Interfaces;

namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class Caretaker
    {      
        private List<IMemento> _mementos = new List<IMemento>();
        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            this._originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            this._mementos.Add(this._originator.Save());
        }

/*
        public void Undo()
        {
            if(this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            Console.WriteLine("Caretaker: Restoring state to: " + memento.GetState());

            try
            {
                this._originator.Restore(memento);
            }
            catch(Exception)
            {
                this.Undo();
            }
        }
        */

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");
            foreach(var memento in this._mementos)
            {
                Console.WriteLine("Tower Position : (" + memento.GetPositionState().X + ";" + memento.GetPositionState().Y + ") Tower Type : " + memento.GetTypeState());
            }
        }

    }

}