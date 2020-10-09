using System;

namespace WeirdUnitBE.GameLogic
{
    [Serializable]
    public class Position
    {
        public readonly int X;
        public readonly int Y;

        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Position InvertPosition(int mapDimensionX, int mapDimensionY)
        {
            return new Position(mapDimensionX - this.X - 1, mapDimensionY - this.Y - 1);
        }
    }
}