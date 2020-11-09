using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class PowerUpExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic info, GameState gameState)
        {
            List<Tower> affectedTowers = new List<Tower>();
            IPowerUpStrategy strategy;
            PowerUpCreator powerUpCreator;
            PowerUp powerUp;
            List<Tower> targetTowers = new List<Tower>();

            string powerUpType = info.powerUpType;
            string powerUpOwner = info.powerUpOwner;

            switch (powerUpType)
            {
                case Constants.JsonCommands.ClientCommands.ATTACKING_TOWER_POWER_UP:
                    strategy = new AttackingTowerPowerUpStrategy();
                    powerUpCreator = new AttackingTowerPowerUpCreator();
                    targetTowers = gameState.GetAttackingTowers().Where(tower => tower.owner == powerUpOwner).ToList();
                    break;
                case Constants.JsonCommands.ClientCommands.REGENERATING_TOWER_POWER_UP:
                    strategy = new RegeneratingTowerPowerUpStrategy();
                    powerUpCreator = new RegeneratingTowerPowerUpCreator();
                    targetTowers = gameState.GetRegeneratingTowers().Where(tower => tower.owner == powerUpOwner).ToList();
                    break;
                case Constants.JsonCommands.ClientCommands.TOWER_DEFENCE_POWER_UP:
                    strategy = new TowerDefencePowerUpStrategy();
                    powerUpCreator = new TowerDefencePowerUpCreator();
                    targetTowers = gameState.GetAllTowers().Where(tower => tower.owner == powerUpOwner).ToList();
                    break;
                default:
                    return null;
            }

            powerUp = powerUpCreator.CreatePowerUp();
            strategy.ExecuteStrategy(powerUp, targetTowers, (_affectedTowers) =>
            {
                affectedTowers = _affectedTowers;
            });
            
            gameState.UpdateTowers(affectedTowers);

            return affectedTowers;
        }
    }
}