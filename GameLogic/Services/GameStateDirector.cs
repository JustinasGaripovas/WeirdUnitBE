using WeirdUnitBE.GameLogic.Services.Interfaces;

namespace WeirdUnitBE.GameLogic.Services
{
    public class GameStateDirector
    {
        GameState result = new GameState();

        private IGameStateBuilder _gameStateBuilder; 
        
        public GameStateDirector(IGameStateBuilder gameStateBuilder, string user1, string user2)
        {
            _gameStateBuilder = gameStateBuilder;
            
            BuildPowerUps();
            BuildTowers(user1, user2);
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