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
using WeirdUnitGame.GameLogic.State.ConcreteStates;
using WeirdUnitGame.GameLogic.Visitor;
using WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors;

namespace WeirdUnitBE.GameLogic
{
    public class PowerUpExecutive : IGameStateExecutable
    {
        private dynamic payload;
        private PowerUp powerUp;
        private IPowerUpStrategy strategy;
        private List<Tower> targetTowers;
        private bool isResponse = false;
        PowerUpCreator powerUpCreator;

        private RegeneratingPowerUpVisitor _regeneratingPowerUpVisitor = new RegeneratingPowerUpVisitor();
        private TowerDefencePowerUpVisitor _towerDefencePowerUpVisitor = new TowerDefencePowerUpVisitor();
        
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            SetPayloadFromArgs(args);
        
            DetermineExecutionSettings(gameState);
            Execute();
            
            return FormatCommand(powerUp, targetTowers);
        }

        private void SetPayloadFromArgs(dynamic args) {
            payload = args.jsonObj.payload; 
        }

        private void DetermineExecutionSettings(GameState gameState) {
            
            string powerUpType = payload.type;
            string powerUpOwner = payload.uuid;
            
            
            switch(powerUpType) {
                case Constants.JsonCommands.ClientCommands.ATTACKING_TOWER_POWER_UP:
                    ConfigureAttackingTowerPowerUpExecution(powerUpOwner, gameState);
                    break;
                case Constants.JsonCommands.ClientCommands.REGENERATING_TOWER_POWER_UP:
                    ConfigureRegeneratingTowerPowerUpExecution(powerUpOwner, gameState);
                    break;
                case Constants.JsonCommands.ClientCommands.TOWER_DEFENCE_POWER_UP:
                    ConfigureTowerDefencePowerUpExecution(powerUpOwner, gameState);
                    break;
                default:
                    throw new InvalidPowerUpException("Invalid PowerUp");
            }
        }

        private void ConfigureAttackingTowerPowerUpExecution(string powerUpOwner, GameState gameState)
        {
            strategy = new AttackingTowerPowerUpStrategy();
            powerUpCreator = new AttackingTowerPowerUpCreator();
            targetTowers = gameState.GetAttackingTowers().Where(tower => tower.owner == powerUpOwner).ToList();
        }

        private void ConfigureRegeneratingTowerPowerUpExecution(string powerUpOwner, GameState gameState)
        {
            strategy = new RegeneratingTowerPowerUpStrategy();
            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            targetTowers = gameState.GetRegeneratingTowers().Where(tower => tower.owner == powerUpOwner).ToList();
            
            PowerUp foundPowerUp = gameState.GetAllPowerUps().Where(powerUp => powerUp.GetType() == typeof(RegeneratingTowerPowerUp)).First();
            
            HandleState(foundPowerUp, _regeneratingPowerUpVisitor);
            
            powerUp = foundPowerUp;
        }

        private void HandleState(PowerUp foundPowerUp, IVisitor visitor)
        {
            if (foundPowerUp.state.GetType() == typeof(ReadyState))
            {
                isResponse = false;
                ApplyVisitor(visitor);
                Console.WriteLine("We apply regenerating visitor");
                foundPowerUp.TransitionTo(new CooldownState());
            }
            else
            {
                isResponse = true;
                Console.WriteLine("We cant apply power up its still cooling down");
                foundPowerUp.TransitionTo(new ReadyState());
            }
        }

        private void ConfigureTowerDefencePowerUpExecution(string powerUpOwner, GameState gameState)
        {
            strategy = new TowerDefencePowerUpStrategy();
            powerUpCreator = new TowerDefencePowerUpCreator();
            targetTowers = gameState.GetAllTowers().Where(tower => tower.owner == powerUpOwner).ToList();
            PowerUp foundPowerUp = gameState.GetAllPowerUps().Where(powerUp => powerUp.GetType() == typeof(TowerDefencePowerUp)).First();

            HandleState(foundPowerUp, _towerDefencePowerUpVisitor);
            
            powerUp = foundPowerUp;
        }

        private void Execute()
        {
            strategy.ExecuteStrategy(powerUp, targetTowers);
        }

        private object FormatCommand(PowerUp powerUp, List<Tower> targetTowers)
        {
            JsonMessageFormatterTemplate formatter = new JsonPowerUpMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(targetTowers, powerUp, isResponse);

            return buffer;  
        }

        private void ApplyVisitor(IVisitor visitor)
        {
            foreach (Tower tower in targetTowers)
            {
                tower.Accept(visitor);
            }
        }
    }
}