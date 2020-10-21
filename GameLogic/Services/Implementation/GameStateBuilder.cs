using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic.Map;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.Services.Interfaces;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.DefaultTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.FastTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.StrongTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Factories;
using WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories;

// ReSharper disable once CheckNamespace
namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class GameStateBuilder: IGameStateBuilder
    {
        private List<PowerUp> allPowerUps = new List<PowerUp>();
        private ConcurrentDictionary<Position, Tower> positionToTowerDict = new ConcurrentDictionary<Position, Tower>();

        public GameStateBuilder() { }

        public List<PowerUp> GeneratePowerUps()
        {
            PowerUpCreator powerUpCreator = new AttackingTowerPowerUpCreator();
            PowerUp powerUp = powerUpCreator.CreatePowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            powerUp = powerUpCreator.CreatePowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new TowerDefencePowerUpCreator();
            powerUp = powerUpCreator.CreatePowerUp();
            allPowerUps.Add(powerUp);

            return allPowerUps;
        }

        public ConcurrentDictionary<Position, Tower> GenerateTowers(string user1, string user2)
        {
            SetInitialTower(user1, MapService.GetInitialTowerPosition());
            SetInitialTower(user2, MapService.GetInitialEnemyTowerPosition());
            
            SetNeutralTowers();
            
            return positionToTowerDict;
        }

        private void SetNeutralTowers()
        {
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            
            foreach (Position position in MapService.GetDefaultMap())
            {
                Tower tower = abstractTowerFactory.CreateRegeneratingTower();
                tower.position = position;
                tower.unitCount = 10;
                positionToTowerDict.TryAdd(tower.position, tower);
            }
        }

        private void SetInitialTower(string user, Position initialPosition)
        {
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            Tower initialTower = abstractTowerFactory.CreateRegeneratingTower();
            initialTower.position = initialPosition;
            initialTower.unitCount = 50;
            initialTower.owner = user;
            positionToTowerDict.TryAdd(initialTower.position, initialTower);
        }
    }
}