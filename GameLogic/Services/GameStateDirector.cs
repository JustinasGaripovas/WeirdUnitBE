using WeirdUnitBE.GameLogic.Services.Interfaces;

namespace WeirdUnitBE.GameLogic.Services
{
    public class GameStateDirector
    {
        GameState result = new GameState();
        
        public GameStateDirector(IGameStateBuilder gameStateBuilder, string user1, string user2)
        {
            result.SetAllPowerUps(gameStateBuilder.GeneratePowerUps());
            
            gameStateBuilder.GenerateUserTowers(user1,user2);
            
            result.PositionToTowerDict = gameStateBuilder.GenerateRandomTowers();
        }

        public GameState GetResult()
        {
            return result;
        }
    }
}