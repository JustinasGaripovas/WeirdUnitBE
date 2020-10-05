
namespace WeirdUnitBE.GameLogic.TowerPackage
{
    abstract class Tower
    {
        private (int, int) Coordinates;

        public (int x, int y) GetCoordinates()
        {
            return Coordinates;
        }

        public void SetCoordinate_x(int x)
        {
            Coordinates.Item1 = x;
        }

        public void SetCoordinate_y(int y)
        {
            Coordinates.Item2 = y;
        }
    }
}