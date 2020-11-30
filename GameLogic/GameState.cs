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
        private GameStateFlyweightInfo flyweightInfo;
        public ConcurrentDictionary<Position, Tower> PositionToTowerDict { get; set; }

        private List<PowerUp> allPowerUpList;
        public GameState() { }

        public GameState(List<Tower> allTowers, List<PowerUp> allPowerUps)
        {
            this.allPowerUpList = allPowerUps;
        }

        public void UpdateTower(Tower tower)
        {
            PositionToTowerDict[tower.position] = tower;
        }

        public void UpdateTowers(List<Tower> towers)
        {
            foreach (Tower tower in towers)
            {
                PositionToTowerDict[tower.position] = tower;
            }
        }

        #region GETTERS
        public Tower GetTowerFromPosition(Position position)
        {
            return PositionToTowerDict[position];
        }
        
        public List<Tower> GetAllTowers()
        {
            return PositionToTowerDict.Values.ToList();
        }

        public List<Tower> GetAttackingTowers() { return GetAllTowers().Where(tower => tower.GetType().BaseType.Equals(typeof(AttackingTower))).ToList<Tower>(); }
        public List<Tower> GetRegeneratingTowers() { return GetAllTowers().Where(tower => tower is RegeneratingTower).ToList<Tower>(); }

        public (int X, int Y) GetMapDimensions()
        {
            return flyweightInfo.mapDimensions;
        }

        public List<PowerUp> GetAllPowerUps()
        {
            return allPowerUpList;
        }

        public double GetGameSpeed()
        {
            return flyweightInfo.gameSpeed;
        }

        #endregion

        #region SETTERS

        public GameState SetAllPowerUps(List<PowerUp> newAllPowerUps)
        {
            this.allPowerUpList = newAllPowerUps;
            return this;
        }

        public void SetFlyweightInfo(GameStateFlyweightInfo flyweightInfo)
        {
            this.flyweightInfo = flyweightInfo;
        }

        #endregion
    }
}