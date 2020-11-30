using WeirdUnitBE.GameLogic.Services.Interfaces;
using WeirdUnitBE.GameLogic.Map;

namespace WeirdUnitBE.GameLogic.Services
{
    public class GameStateDirector
    {
        GameState result;

        private IGameStateBuilder _gameStateBuilder; 
        
        public GameStateDirector(IGameStateBuilder gameStateBuilder, string user1, string user2)
        {
            _gameStateBuilder = gameStateBuilder;
            result = new GameState();
            
            BuildFlyweightInfo(MapService.GetDefaultMapDimensions(), MapService.GetDefaultGameSpeed());
            BuildPowerUps();
            BuildTowers(user1, user2);
        }

        public void BuildFlyweightInfo((int, int) mapDimensions, double gameSpeed)
        {
            result.SetFlyweightInfo(_gameStateBuilder.GenerateFlyweightInfo(mapDimensions, gameSpeed));
            /*
            public static List<Position> GetDefaultMap()
        {
            return GetDefaultMapConnections().Keys.ToList();
        }

        public static (int X, int Y) GetDefaultMapDimensions()
        {
            return (10, 10);
        }*/
        }

        public void BuildPowerUps()
        {
            result.SetAllPowerUps(_gameStateBuilder.GeneratePowerUps());
        }

        public void BuildTowers( string user1, string user2)
        {
            result.PositionToTowerDict = _gameStateBuilder.GenerateTowers(user1, user2);
        }

        public GameState GetResult()
        {
            return result;
        }
    }
}