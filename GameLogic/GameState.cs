using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;

namespace WeirdUnitBE.GameLogic
{
    
    public class GameState
    {
        private readonly (int X, int Y) MAP_DIMENSIONS = (10, 10);

        private List<Tower> allTowerList;

        private List<PowerUp> allPowerUps;

        public Tower initialUser1Tower, initialUser2Tower;

        private static GameState _instance;

        private GameState() { }

        public static GameState GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameState();
            }
            return _instance;
        }

        public void GenerateRandomGameState()
        {
            allTowerList = new List<Tower>();
            allPowerUps = new List<PowerUp>();

            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            GenerateUserTowers(abstractTowerFactory); // Generate initial User towers
            GenerateRandomTowers(abstractTowerFactory); // Generate Other random towers

            PowerUpCreator powerUpCreator;
            GeneratePowerUps(out powerUpCreator);
        }

        private void GeneratePowerUps(out PowerUpCreator powerUpCreator)
        {
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

        private void GenerateUserTowers(AbstractFactory abstractFactory)
        {
            // Generate First user tower
            abstractFactory = new DefaultTowerFactory();
            initialUser1Tower = abstractFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;

            // Generate Second users tower (which is symmetric to First user)
            initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);

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
            return allPowerUps;
        }
        #endregion

        #region SETTERS
            
        #endregion

    }
}