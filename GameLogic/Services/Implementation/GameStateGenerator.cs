using System;
using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.Services.Interfaces;
using WeirdUnitBE.GameLogic.TowerPackage;

// ReSharper disable once CheckNamespace
namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class GameStateGenerator: IGameStateGenerator
    {
        private List<Tower> allTowerList = new List<Tower>();
        private List<PowerUp> allPowerUps = new List<PowerUp>();

        public GameStateGenerator() { }

        public GameState GenerateRandomGameState()
        {
            GenerateUserTowers();
            GeneratePowerUps();
            
            return  new GameState(allTowerList, allPowerUps);
        }

        private void GeneratePowerUps()
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
        }

        private void GenerateRandomTowers()
        {
            int rTowerCount = Randomizer.ReturnRandomInteger(3, 6);
            for (int i = 0; i < rTowerCount; i++)
            {
                Position position = GenerateRandomPosition();
                while(allTowerList.Where(t => t.position.X == position.X && t.position.Y == position.Y).Any())
                {
                    position = GenerateRandomPosition();
                }

                // Generate Random tower
                Tower tower = GenerateRandomTower();
                tower.unitCount = Randomizer.ReturnRandomInteger(0, 51);
                tower.position = position;                
                allTowerList.Add(tower); // cia

                // Generate tower symmetric to the previous tower 
                Tower newTower = tower.ReturnSymmetricTower(10, 10);
                allTowerList.Add(newTower); // cia
            }
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

            Tower randomTower = dictionary[type]; // Based on a generated random Tower type, create a new tower of that type

            return (Tower)dictionary[type]; // Return randomly generated Tower       
        }

        private void GenerateUserTowers()
        {
            // Generate First user tower
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            Tower initialUser1Tower = abstractTowerFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;

            // Generate Second users tower (which is symmetric to First user)
            Tower initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(10, 10);

            // Store both towers su global List
            allTowerList.Add(initialUser1Tower);
            allTowerList.Add(initialUser2Tower);
        }
        private Position GenerateRandomPosition()
        {
            int x = Randomizer.ReturnRandomInteger(0, 10 / 2);
            int y = Randomizer.ReturnRandomInteger(0, 10);
            return new Position(x, y);
        }
    }
}