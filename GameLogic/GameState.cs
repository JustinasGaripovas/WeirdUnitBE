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
using WeirdUnitBE.GameLogic.Services.Implementation;

namespace WeirdUnitBE.GameLogic
{
    public class GameState
    {
        private GameStateFlyweightInfo flyweightInfo;
        public ConcurrentDictionary<Position, Tower> PositionToTowerDict { get; set; }
        public Originator originator;
        public Caretaker caretaker;

        private List<PowerUp> allPowerUpList;
        public GameState()
        { 
            originator = new Originator();
            caretaker = new Caretaker(originator);
        }

        public GameState(List<Tower> allTowers, List<PowerUp> allPowerUps)
        {
            this.allPowerUpList = allPowerUps;

            originator = new Originator();
            caretaker = new Caretaker(originator);
        }

        public void UpdateTower(Tower tower)
        {
            PositionToTowerDict[tower.position] = tower;
            originator.ChangeState(tower.position, tower.type);
            caretaker.Backup();
            caretaker.ShowHistory();
        }

        public void UpdateTowers(List<Tower> towers)
        {
            foreach (Tower tower in towers)
            {
                UpdateTower(tower);
                //PositionToTowerDict[tower.position] = tower;
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

        public List<Tower> GetAttackingTowers() 
        { 
            TowersCollection towersCollection = new TowersCollection(GetAllTowers());
            AttackingTowersIterator attackingIterator = (AttackingTowersIterator)towersCollection.GetAttackingIterator();

            List<Tower> attackingTowers = new List<Tower>();
            for(attackingIterator.First(); !attackingIterator.IsDone(); attackingIterator.MoveNext())
            {
                attackingTowers.Add((Tower)attackingIterator.Current());
                
            }

            return attackingTowers;
            //GetAllTowers().Where(tower => tower.GetType().BaseType.Equals(typeof(AttackingTower))).ToList<Tower>(); 
        }
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