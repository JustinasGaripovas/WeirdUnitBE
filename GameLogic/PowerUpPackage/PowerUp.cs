
using System;
using WeirdUnitGame.GameLogic.State;
using WeirdUnitGame.GameLogic.State.ConcreteStates;

namespace WeirdUnitBE.GameLogic.PowerUpPackage
{
    public abstract class PowerUp : IStateful
    {
        public string type;
        public string owner;
        public State state = new CooldownState();
        public abstract void printInfo();
        public void TransitionTo(State state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this.state = state;
            this.state.SetPowerUp(this);
        }
    }
}