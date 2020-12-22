
using System;
using WeirdUnitGame.GameLogic.State;
using WeirdUnitGame.GameLogic.State.ConcreteStates;

namespace WeirdUnitBE.GameLogic.PowerUpPackage
{
    public abstract class PowerUp : IStateful
    {
        public string type;
        public string owner;
        public int executionTimeInSeconds { get; set; } = 8;
        public State state = new ReadyState();
        public abstract void printInfo();
        public void TransitionTo(State state)
        {
            Console.WriteLine($"Context: Transition to {state.GetType().Name}.");
            this.state = state;
            this.state.SetPowerUp(this);
        }
    }
}