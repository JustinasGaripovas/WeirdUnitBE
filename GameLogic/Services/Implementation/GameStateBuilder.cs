using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            PowerUp powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new TowerDefencePowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);

            return allPowerUps;
        }

        public ConcurrentDictionary<Position, Tower> GenerateRandomTowers()
        {
            int rTowerCount = Randomizer.ReturnRandomInteger(3, 6);
            for (int i = 0; i < rTowerCount; i++)
            {
                Position position = GenerateRandomPosition();
                while(positionToTowerDict.ContainsKey(position))
                {
                    position = GenerateRandomPosition();
                }

                Tower tower = GenerateRandomTower();
                tower.unitCount = Randomizer.ReturnRandomInteger(0, 51);
                tower.position = position;                
                positionToTowerDict.TryAdd(tower.position, tower); 

                Tower symmetricTower = tower.ReturnSymmetricTower(10, 10);
                positionToTowerDict.TryAdd(symmetricTower.position, symmetricTower);
            }
            
            return positionToTowerDict;
        }

        private Tower GenerateRandomTower()
        {
            Type type = Randomizer.ReturnRandomType<Tower>(); // Generate a random type

            IDictionary<Type, Tower> dictionary = new Dictionary<Type, Tower>();

            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            dictionary.Add(typeof(DefaultAttackingTower), abstractTowerFactory.CreateAttackingTower());
            dictionary.Add(typeof(DefaultRegeneratingTower), abstractTowerFactory.CreateRegeneratingTower());

            abstractTowerFactory = new FastTowerFactory();
            dictionary.Add(typeof(FastAttackingTower), abstractTowerFactory.CreateAttackingTower());
            dictionary.Add(typeof(FastRegeneratingTower), abstractTowerFactory.CreateRegeneratingTower());

            abstractTowerFactory = new StrongTowerFactory();
            dictionary.Add(typeof(StrongAttackingTower), abstractTowerFactory.CreateAttackingTower());
            dictionary.Add(typeof(StrongRegeneratingTower), abstractTowerFactory.CreateRegeneratingTower());

            return dictionary[type]; 
        }

        public void GenerateUserTowers(string user1, string user2)
        {
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            Tower initialUser1Tower = abstractTowerFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;
            initialUser1Tower.owner = user1;

            Tower initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(10, 10);
            initialUser2Tower.owner = user2;

            positionToTowerDict.TryAdd(initialUser1Tower.position, initialUser1Tower);
            positionToTowerDict.TryAdd(initialUser2Tower.position, initialUser2Tower);
        }
        private Position GenerateRandomPosition()
        {
            int x = Randomizer.ReturnRandomInteger(0, 10 / 2);
            int y = Randomizer.ReturnRandomInteger(0, 10);
            return new Position(x, y);
        }
    }
}