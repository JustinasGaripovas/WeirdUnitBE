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
        public ConcurrentDictionary<Position, Tower> PositionToTowerDict { get; set; }

        private List<PowerUp> allPowerUpList;

        public GameState() { }

        public GameState(List<Tower> allTowers, List<PowerUp> allPowerUps)
        {
            this.allPowerUpList = allPowerUps;    
        }

        public void Execute(Tower towerFrom, Tower towerTo, out List<Tower> affectedTowers)
        {
            IStrategy strategy;

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
                PositionToTowerDict[towerFrom.position],
                PositionToTowerDict[towerTo.position]
            };
        }

        void UpdateTowers(Tower tower1, Tower tower2)
        {
            PositionToTowerDict[tower1.position] = tower1;
            PositionToTowerDict[tower2.position] = tower2;
        }
        
        #region GETTERS
        public List<Tower> GetAllTowers()
        {
            return PositionToTowerDict.Values.ToList();
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
         
        public GameState SetAllPowerUps(List<PowerUp> newAllPowerUps)
        {
            this.allPowerUpList = newAllPowerUps;
            return this;
        }
        
        #endregion

    }
}