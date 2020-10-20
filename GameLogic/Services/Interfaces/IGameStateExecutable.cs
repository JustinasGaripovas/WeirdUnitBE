using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public interface IGameStateExecutable
    {
        object ExecuteCommand(dynamic info, GameState gameState);
    }
}