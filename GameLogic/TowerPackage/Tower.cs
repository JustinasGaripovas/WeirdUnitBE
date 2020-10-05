using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    public abstract class Tower
    {
        private (int, int) Coordinates;

        public int unitCount = 0;
        public int X;
        public int Y;

        public string type;

        public (int x, int y) GetCoordinates()
        {
            return Coordinates;
        }

        public void SetCoordinate_x(int x)
        {
            Coordinates.Item1 = x;
            X = x;
        }

        public void SetCoordinate_y(int y)
        {
            Coordinates.Item2 = y;
            Y = y;
        }
    }
}