using System;
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
        public readonly (int X, int Y) MAP_DIMENSIONS = (10, 10);



        AbstractFactory defaultTowerFactory = new DefaultTowerFactory();
        AbstractFactory strongTowerFactory = new StrongTowerFactory();
        AbstractFactory fastTowerFactory = new FastTowerFactory();

        ConcurrentDictionary<(int, int), Tower> allTowers = new ConcurrentDictionary<(int, int), Tower>(); // CIA ROKO

        List<PowerUp> allPowerUps = new List<PowerUp>();

        public Tower initialUser1Tower, initialUser2Tower;

        public GameState() { }

        public void GenerateRandomGameState()
        {
            #region Generate towers  
            // Generate initial user tower
            initialUser1Tower = defaultTowerFactory.CreateRegeneratingTower();
            initialUser1Tower.SetCoordinate_x(0);
            initialUser1Tower.SetCoordinate_y(4);
            initialUser1Tower.unitCount = 50;

            initialUser2Tower = defaultTowerFactory.CreateRegeneratingTower();
            initialUser2Tower.SetCoordinate_x(9 - initialUser1Tower.GetCoordinates().x);
            initialUser2Tower.SetCoordinate_y(9 - initialUser1Tower.GetCoordinates().y);
            initialUser2Tower.unitCount = 50;

            allTowers.TryAdd(initialUser1Tower.GetCoordinates(), initialUser1Tower);
            allTowers.TryAdd(initialUser2Tower.GetCoordinates(), initialUser2Tower);

            int rTowerCount = GenerateRandomInt(3, 6, DateTime.Now.Millisecond);

            for (int i = 0; i < rTowerCount; i++)
            {
                int seed = DateTime.Now.Millisecond;
                int rX = GenerateRandomInt(0, 5, seed);
                int rY = GenerateRandomInt(0, 10, seed + 1);
                while (allTowers.ContainsKey((rX, rY)))
                {
                    rX = GenerateRandomInt(0, 5, seed + 2);
                    rY = GenerateRandomInt(0, 10, seed + 3);
                    seed++;
                }
                Tower tower = defaultTowerFactory.CreateRegeneratingTower();
                tower.SetCoordinate_x(rX);
                tower.SetCoordinate_y(rY);
                //tower.type = tower.GetType().ToString().Substring();
                allTowers.TryAdd(tower.GetCoordinates(), tower);

                Tower newTower = defaultTowerFactory.CreateRegeneratingTower();
                newTower.SetCoordinate_x(9 - rX);
                newTower.SetCoordinate_y(9 - rY);
                allTowers.TryAdd(newTower.GetCoordinates(), newTower);
            }
            #endregion
            // Generate random unoccupied Towers
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



        public List<PowerUp> GetAllPowerUps()
        {
            return allPowerUps;
        }

        public int GenerateRandomInt(int from, int to, int seed)
        {
            Random r = new Random(seed);
            int rInt = r.Next(from, to); //for ints
            return rInt;
        }
        public (int X, int Y) Get_MAP_DIMENSIONS()
        {
            return MAP_DIMENSIONS;
        }

        public ConcurrentDictionary<(int, int), Tower> GetAllTowers()
        {
            return allTowers;
        }

    }
}