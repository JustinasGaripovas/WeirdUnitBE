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
            BuildUserTowers(user1, user2);
            BuildRandomTowers();
        }

        public void BuildPowerUps()
        {
            result.SetAllPowerUps(_gameStateBuilder.GeneratePowerUps());
        }

        public void BuildUserTowers(string user1, string user2)
        {
            _gameStateBuilder.GenerateUserTowers(user1,user2);
        }

        public void BuildRandomTowers()
        {
            result.PositionToTowerDict = _gameStateBuilder.GenerateRandomTowers();
        }

        public GameState GetResult()
        {
            return result;
        }
    }
}