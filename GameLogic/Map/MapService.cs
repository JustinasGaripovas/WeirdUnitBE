using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Map
{
    public static class MapService
    {
        private static Position initialTowerPosition = new Position(0,4);
        private static Position initialEnemyTowerPosition = new Position(9,5);

        public static Position GetInitialTowerPosition()
        {
            return initialTowerPosition;
        }

        public static Position GetInitialEnemyTowerPosition()
        {
            return initialEnemyTowerPosition;
        }
        
        public static IDictionary<Position, Position[]> GetDefaultMapConnections()
        {
            IDictionary<Position, Position[]> defaultMap = new Dictionary<Position, Position[]>();

            defaultMap[new Position(0, 4)] =
                new Position[]
                {
                    new Position(0, 2),
                    new Position(2, 2),
                    new Position(2, 6),
                };

            defaultMap[new Position(0, 2)] =
                new Position[]
                {
                    new Position(0, 4),
                    new Position(2, 2),
                };

            defaultMap[new Position(2, 2)] =
                new Position[]
                {
                    new Position(0, 4),
                    new Position(0, 2),
                    new Position(4, 1),
                    new Position(3, 4),
                };

            defaultMap[new Position(4, 1)] =
                new Position[]
                {
                    new Position(7, 3),
                    new Position(2, 2),
                    new Position(3, 4),
                };

            defaultMap[new Position(3, 4)] =
                new Position[]
                {
                    new Position(4, 1),
                    new Position(2, 2),
                    new Position(6, 5),
                    new Position(2, 6),
                };

            defaultMap[new Position(2, 6)] =
                new Position[]
                {
                    new Position(3, 4),
                    new Position(0, 4),
                    new Position(5, 8),
                };

            defaultMap[new Position(7, 3)] =
                new Position[]
                {
                    new Position(4, 1),
                    new Position(9, 5),
                    new Position(6, 5),
                };

            defaultMap[new Position(9, 5)] =
                new Position[]
                {
                    new Position(9, 7),
                    new Position(7, 7),
                    new Position(7, 3),
                };

            defaultMap[new Position(9, 7)] =
                new Position[]
                {
                    new Position(9, 5),
                    new Position(7, 7),
                };

            defaultMap[new Position(7, 7)] =
                new Position[]
                {
                    new Position(7, 3),
                    new Position(7, 7),
                    new Position(5, 8),
                    new Position(3, 4),
                };

            defaultMap[new Position(5, 8)] =
                new Position[]
                {
                    new Position(2, 6),
                    new Position(6, 5),
                    new Position(7, 7),
                };
            
            defaultMap[new Position(6, 5)] =
                new Position[]
                {
                    new Position(7, 3),
                    new Position(7, 7),
                    new Position(5, 8),
                    new Position(3, 4),
                };

            return defaultMap;
        }

        public static List<Position> GetDefaultMap()
        {
            List<Position> allTowerPositions = GetDefaultMapConnections().Keys.ToList();
            allTowerPositions.Remove(initialTowerPosition);
            allTowerPositions.Remove(initialEnemyTowerPosition);

            return allTowerPositions;
        }
    }
}