using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;

namespace WeirdUnitBE.GameLogic.Services.Implementation
{
    public class GameStateGenerator
    {
        private List<Tower> allTowerList = new List<Tower>();
        private List<PowerUp> allPowerUps = new List<PowerUp>();

        private GameStateGenerator() { }

        public void GenerateRandomGameState(): GameState
        {
            GenerateUserTowers();
            GenerateRandomTowers();
            GeneratePowerUps();

            return new GameState(allTowerList, allPowerUps);
        }

        private void GeneratePowerUps()
        {
            PowerUpCreator powerUpCreator = new PowerUpCreator();

            powerUpCreator = new AttackingTowerPowerUpCreator();
            PowerUp powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);

            powerUpCreator = new TowerDefencePowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUps.Add(powerUp);
        }

        private void GenerateRandomTowers(AbstractFactory abstractTowerFactory)
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
                Tower tower = GenerateRandomTower(abstractTowerFactory);
                tower.unitCount = Randomizer.ReturnRandomInteger(0, 51);
                tower.position = position;                
                allTowerList.Add(tower); // cia

                // Generate tower symmetric to the previous tower 
                Tower newTower = tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);
                allTowerList.Add(newTower); // cia
            }
        }

        private Tower GenerateRandomTower()
        {
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();

            Type type = Randomizer.ReturnRandomType<Tower>(); // Generate a random type

            IDictionary<Type, Tower> dictionary = new Dictionary<Type, Tower>();

            abstractFactory = new DefaultTowerFactory();
            dictionary.Add(typeof(DefaultAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(DefaultRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            abstractFactory = new FastTowerFactory();
            dictionary.Add(typeof(FastAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(FastRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            abstractFactory = new StrongTowerFactory();
            dictionary.Add(typeof(StrongAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(StrongRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            Tower randomTower = dictionary[type]; // Based on a generated random Tower type, create a new tower of that type

            return (Tower)dictionary[type]; // Return randomly generated Tower       
        }

        private void GenerateUserTowers()
        {
            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();

            // Generate First user tower
            abstractFactory = new DefaultTowerFactory();
            Tower initialUser1Tower = abstractFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;

            // Generate Second users tower (which is symmetric to First user)
            Tower initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);

            // Store both towers su global List
            allTowerList.Add(initialUser1Tower);
            allTowerList.Add(initialUser2Tower);
        }
        private Position GenerateRandomPosition()
        {
            int x = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.X / 2);
            int y = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.Y);
            return new Position(x, y);
        }
    }
}