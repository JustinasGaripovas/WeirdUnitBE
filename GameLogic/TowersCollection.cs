using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic
{
    public class TowersCollection : IAggregator
    {
        List<Tower> _towersCollection;        

        public TowersCollection(List<Tower> towersCollection)
        {
            _towersCollection = towersCollection;
        }

        public Tower this[int index] => _towersCollection[index];     

        public List<Tower> GetCollection()
        {
            return this._towersCollection;
        }        

        public IIterator GetAttackingIterator()
        {
            return new AttackingTowersIterator(this);
        }

        public IIterator GetRegeneratingIterator()
        {
            return new RegeneratingTowersIterator(this);
        }

    }
}