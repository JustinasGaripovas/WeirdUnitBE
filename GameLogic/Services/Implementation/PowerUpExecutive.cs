using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps;
using WeirdUnitBE.GameLogic.Strategies;
using WeirdUnitBE.Middleware.JsonHandling;

namespace WeirdUnitBE.GameLogic
{
    public class PowerUpExecutive : IGameStateExecutable
    {
        private dynamic payload;
        private PowerUp powerUp;
        private IPowerUpStrategy strategy;
        private List<Tower> targetTowers;

        public object ExecuteCommand(dynamic args, GameState gameState)
        {       
            SetPayloadFromArgs(args);
            DetermineExecutionSettings(gameState);
            Execute();
            
            return FormatCommand(targetTowers);
        }

        private void SetPayloadFromArgs(dynamic args) {
            payload = args.jsonObj.payload; 
        }

        private void DetermineExecutionSettings(GameState gameState) {
            PowerUpCreator powerUpCreator;
            Console.WriteLine((string)payload.type);
            string powerUpType = payload.type;
            string powerUpOwner = payload.uuid;

            switch(powerUpType) {
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
                    throw new InvalidPowerUpException("Invalid PowerUp");
            }

            powerUp = powerUpCreator.CreatePowerUp();
        }

        private void Execute()
        {
            strategy.ExecuteStrategy(powerUp, targetTowers);
        }

        private object FormatCommand(List<Tower> targetTowers)
        {
            JsonMessageFormatterTemplate formatter = new JsonPowerUpMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(targetTowers);

            return buffer;  
        }
    }
}