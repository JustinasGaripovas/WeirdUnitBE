using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.GameLogic
{
    
    public class GameState
    {
        private readonly (int X, int Y) MAP_DIMENSIONS = (10, 10);

        private List<Tower> allTowerList;

        public ConcurrentDictionary<Position, Tower> positionToTowerDict;

        private List<PowerUp> allPowerUpList;

        public Tower initialUser1Tower, initialUser2Tower;

        //private static GameState _instance;

        public GameState() { }

        public GameState(List<Tower> allTowers, List<PowerUp> allPowerUps)
        {
            this.allTowerList = allTowers;
            this.allPowerUpList = allPowerUps;    
        }

        public void GenerateRandomGameState(string user1, string user2)
        {
            allTowerList = new List<Tower>();
            positionToTowerDict = new ConcurrentDictionary<Position, Tower>();
            allPowerUpList = new List<PowerUp>();

            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            GenerateUserTowers(abstractTowerFactory, user1, user2); // Generate initial User towers
            GenerateRandomTowers(abstractTowerFactory); // Generate Other random towers

            PowerUpCreator powerUpCreator;
            GeneratePowerUps(out powerUpCreator);
        }

        private void GeneratePowerUps(out PowerUpCreator powerUpCreator)
        {
            powerUpCreator = new AttackingTowerPowerUpCreator();
            PowerUp powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);

            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);

            powerUpCreator = new TowerDefencePowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);
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
                allTowerList.Add(tower);
                positionToTowerDict.TryAdd(tower.position, tower);

                // Generate tower symmetric to the previous tower 
                Tower newTower = tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);
                allTowerList.Add(newTower);
                positionToTowerDict.TryAdd(newTower.position, newTower);
            }
        }

        private Tower GenerateRandomTower(AbstractFactory abstractFactory)
        {
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

        private void GenerateUserTowers(AbstractFactory abstractFactory, string user1, string user2)
        {
            // Generate First user tower
            abstractFactory = new DefaultTowerFactory();
            Tower initialUser1Tower = initialUser1Tower = abstractFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;
            initialUser1Tower.owner = user1;

            // Generate Second users tower (which is symmetric to First user)
            Tower initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);
            initialUser2Tower.owner = user2;

            // Store both towers su global List
            allTowerList.Add(initialUser1Tower);
            allTowerList.Add(initialUser2Tower);
            positionToTowerDict.TryAdd(initialUser1Tower.position, initialUser1Tower);
            positionToTowerDict.TryAdd(initialUser2Tower.position, initialUser2Tower);
        }
        private Position GenerateRandomPosition()
        {
            int x = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.X / 2);
            int y = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.Y);
            return new Position(x, y);
        }

        public void PerformAttack(Position positionFrom, Position positionTo, out List<Tower> affectedTowers)
        {
            Tower towerFrom = positionToTowerDict[positionFrom];
            Tower towerTo = positionToTowerDict[positionTo];

            int attackUnitCount = towerFrom.unitCount / 2;

            positionToTowerDict[towerFrom.position].unitCount -= attackUnitCount; // Reduce units from attacking tower

            positionToTowerDict[towerTo.position].unitCount = Math.Abs(towerTo.unitCount - attackUnitCount); // set occupied towers new unitCount
            positionToTowerDict[towerTo.position].owner = towerFrom.owner; // change tower's owner
            
            affectedTowers = new List<Tower>()
            {
                positionToTowerDict[towerFrom.position],
                positionToTowerDict[towerTo.position]
            };
        }

        public void ReinforceFriendly(Position positionFrom, Position positionTo, out List<Tower> affectedTowers)
        {
            Tower towerFrom = positionToTowerDict[positionFrom];
            Tower towerTo = positionToTowerDict[positionTo];

            int attackUnitCount = towerFrom.unitCount / 2;

            positionToTowerDict[towerFrom.position].unitCount -= attackUnitCount; // Reduce units from attacking tower

            positionToTowerDict[towerTo.position].unitCount = attackUnitCount + towerTo.unitCount; // set occupied towers new unitCount
            positionToTowerDict[towerTo.position].owner = towerFrom.owner; // change tower's owner
            
            affectedTowers = new List<Tower>()
            {
                positionToTowerDict[towerFrom.position],
                positionToTowerDict[towerTo.position]
            };
        }
        
        #region GETTERS

        public List<Tower> getAllTowerList()
        {
            return allTowerList;
        }
        public (int X, int Y) Get_MAP_DIMENSIONS()
        {
            return MAP_DIMENSIONS;
        }

        public List<PowerUp> GetAllPowerUps()
        {
            return allPowerUpList;
        }
        #endregion

        #region SETTERS
            
        #endregion

    }
}