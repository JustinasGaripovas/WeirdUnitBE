using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.DefaultTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.FastTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.StrongTowers;
using WeirdUnitBE.GameLogic.TowerPackage.Factories;
using WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.Strategies;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.GameLogic
{
    
    public class GameState
    {
        private readonly (int X, int Y) MAP_DIMENSIONS = (10, 10);

        public ConcurrentDictionary<Position, Tower> positionToTowerDict;

        private List<PowerUp> allPowerUpList;

        public GameState() { }

        public GameState(List<Tower> allTowers, List<PowerUp> allPowerUps)
        {
            this.allPowerUpList = allPowerUps;    
        }

        public void GenerateRandomGameState(string user1, string user2)
        {
            positionToTowerDict = new ConcurrentDictionary<Position, Tower>();
            allPowerUpList = new List<PowerUp>();

            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();
            GenerateUserTowers(abstractTowerFactory, user1, user2);
            GenerateRandomTowers(abstractTowerFactory);

            PowerUpCreator powerUpCreator;
            GeneratePowerUps(out powerUpCreator);
        }

        private void GeneratePowerUps(out PowerUpCreator powerUpCreator)
        {
            powerUpCreator = new AttackingTowerPowerUpCreator();
            PowerUp powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);

            powerUpCreator = new RegeneratingTowerPowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);

            powerUpCreator = new TowerDefencePowerUpCreator();
            powerUp = powerUpCreator.createPowerUp();
            allPowerUpList.Add(powerUp);
        }

        private void GenerateRandomTowers(AbstractFactory abstractTowerFactory)
        {
            int rTowerCount = Randomizer.ReturnRandomInteger(3, 6);
            for (int i = 0; i < rTowerCount; i++)
            {
                Position position = GenerateRandomPosition();
                
                while(positionToTowerDict.ContainsKey(position))
                {
                    position = GenerateRandomPosition();
                }

                Tower tower = GenerateRandomTower(abstractTowerFactory);
                tower.unitCount = Randomizer.ReturnRandomInteger(0, 51);
                tower.position = position;                
                
                positionToTowerDict.TryAdd(tower.position, tower); 
                Tower symmetricTower = tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);
                positionToTowerDict.TryAdd(symmetricTower.position, symmetricTower);
            }
        }

        private Tower GenerateRandomTower(AbstractFactory abstractFactory)
        {
            Type type = Randomizer.ReturnRandomType<Tower>();

            IDictionary<Type, Tower> dictionary = new Dictionary<Type, Tower>();

            abstractFactory = new DefaultTowerFactory();
            dictionary.Add(typeof(DefaultAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(DefaultRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            abstractFactory = new FastTowerFactory();
            dictionary.Add(typeof(FastAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(FastRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            abstractFactory = new StrongTowerFactory();
            dictionary.Add(typeof(StrongAttackingTower), abstractFactory.CreateAttackingTower());
            dictionary.Add(typeof(StrongRegeneratingTower), abstractFactory.CreateRegeneratingTower());

            Tower randomTower = dictionary[type];

            return (Tower)dictionary[type];      
        }

        private void GenerateUserTowers(AbstractFactory abstractFactory, string user1, string user2)
        {
            abstractFactory = new DefaultTowerFactory();
            Tower initialUser1Tower = abstractFactory.CreateRegeneratingTower();
            initialUser1Tower.position = new Position(0, 4);
            initialUser1Tower.unitCount = 50;
            initialUser1Tower.owner = user1;

            Tower initialUser2Tower = initialUser1Tower.ReturnSymmetricTower(MAP_DIMENSIONS.X, MAP_DIMENSIONS.Y);
            initialUser2Tower.owner = user2;

            positionToTowerDict.TryAdd(initialUser1Tower.position, initialUser1Tower);
            positionToTowerDict.TryAdd(initialUser2Tower.position, initialUser2Tower);
        }
        private Position GenerateRandomPosition()
        {
            int x = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.X / 2);
            int y = Randomizer.ReturnRandomInteger(0, MAP_DIMENSIONS.Y);
            return new Position(x, y);
        }

        public void ExecuteMoveTo(Tower towerFrom, Tower towerTo, out List<Tower> affectedTowers)
        {
            IMoveStrategy strategy;

            if(towerFrom.owner == towerTo.owner)
            {
                strategy = new ReinforceStrategy();
            }
            else
            {
                strategy = new AttackStrategy();
            }

            strategy.ExecuteStrategy(towerFrom, towerTo, (affectedTowerFrom, affectedTowerTo) =>
            {
                UpdateTowers(affectedTowerFrom, affectedTowerTo);
            });


            affectedTowers = new List<Tower>()
            {
                positionToTowerDict[towerFrom.position],
                positionToTowerDict[towerTo.position]
            };
        }

        public void ExecutePowerUp(string powerUpType, string powerUpOwner, out List<Tower> affectedTowers)
        {
            affectedTowers = new List<Tower>();
            IPowerUpStrategy strategy;
            PowerUpCreator powerUpCreator;
            PowerUp powerUp;

            switch(powerUpType)
            {
                case Constants.JsonCommands.ClientCommands.ATTACKING_TOWER_POWER_UP:
                    strategy = new AttackingTowerPowerUpStrategy();
                    powerUpCreator = new AttackingTowerPowerUpCreator();
                    powerUp = powerUpCreator.createPowerUp();
                break;
                default:
                    return;
            }          

            List<Tower> tempTowers = new List<Tower>();
            List<Tower> myAttackingTowers = GetAllTowers().Where(tower => tower is AttackingTower && tower.owner == powerUpOwner).ToList<Tower>();
            strategy.ExecuteStrategy(powerUp, attackingTowers, (_affectedTowers) => 
            {
                tempTowers = _affectedTowers.Intersect(myAttackingTowers).ToList<Tower>();                
            });
            affectedTowers = tempTowers;
        }

        void UpdateTowers(Tower tower1, Tower tower2)
        {
            positionToTowerDict[tower1.position] = tower1;
            positionToTowerDict[tower2.position] = tower2;
        }
        
        #region GETTERS
        public List<Tower> GetAllTowers()
        {
            return positionToTowerDict.Values.ToList();
        }
        public (int X, int Y) Get_MAP_DIMENSIONS()
        {
            return MAP_DIMENSIONS;
        }

        public List<PowerUp> GetAllPowerUps()
        {
            return allPowerUpList;
        }
        #endregion

        #region SETTERS
            
        #endregion

    }
}