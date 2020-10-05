using System;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections.Concurrent;

namespace WeirdUnitBE.GameLogic
{
    public class GameState
    {
        public readonly (int X, int Y) MAP_DIMENSIONS = (10, 10);

        AbstractFactory defaultTowerFactory = new DefaultTowerFactory();
        AbstractFactory strongTowerFactory = new StrongTowerFactory();
        AbstractFactory fastTowerFactory = new FastTowerFactory();

        ConcurrentDictionary<(int, int), Tower> allTowers;

        public GameState(){}

        public void GenerateRandomGameState()
        {
            // Generate initial user tower
            Tower userDefaultRegeneratingTower = defaultTowerFactory.CreateRegeneratingTower();
            userDefaultRegeneratingTower.SetCoordinate_x(0);
            userDefaultRegeneratingTower.SetCoordinate_y(4);

            int rTowerCount = GenerateRandomInt(3, 6);

            for(int i=0; i<rTowerCount; i++)
            {
                int rX = GenerateRandomInt(0, 10);
                int rY = GenerateRandomInt(0, 10);
                while(allTowers.ContainsKey((rX, rY)))
                {
                    rX = GenerateRandomInt(0, 10);
                    rY = GenerateRandomInt(0, 10);
                }
                Tower tower = defaultTowerFactory.CreateRegeneratingTower();
                tower.SetCoordinate_x(rX);
                tower.SetCoordinate_y(rY);
                allTowers.TryAdd(tower.GetCoordinates(), tower);
            }


            // Generate random unoccupied Towers
        }

        public int GenerateRandomInt(int from, int to)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int rInt = r.Next(from, to); //for ints
            return rInt;
        }
        public (int X, int Y) Get_MAP_DIMENSIONS()
        {
            return MAP_DIMENSIONS;
        }
        
    }
}