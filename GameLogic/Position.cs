using System;

namespace WeirdUnitBE.GameLogic
{
    public class Position
    {
        public readonly int X;
        public readonly int Y;

        public Position(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }

        public double DistanceToPosition(Position position)
        {
            return Math.Sqrt(Math.Pow(this.X - position.X, 2) +
                Math.Pow(this.Y - position.Y, 2));
        }

        public Position SymmetricPosition(int mapDimensionX, int mapDimensionY)
        {
            return new Position(mapDimensionX - this.X - 1, mapDimensionY - this.Y - 1);
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else 
            {
                Position p = (Position) obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

    }
}