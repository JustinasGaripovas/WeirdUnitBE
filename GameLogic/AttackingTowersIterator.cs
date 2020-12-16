using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitGame.GameLogic;

namespace WeirdUnitBE.GameLogic
{
    public class AttackingTowersIterator : IIterator
    {
        private TowersCollection _collection;
        private int _position;

        public AttackingTowersIterator(TowersCollection collection)
        {
            _collection = collection;           
            _position = -1;
        }
        public int Key()
        {
            return this._position;
        }
        public object Current()
        {
            return this._collection[_position];
        }

        public void First()
        {           
            this._position = this._collection.GetCollection().TakeWhile(tower => !(tower is AttackingTower)).Count();
        }
        public bool IsDone()
        {
            return this._position < 0 && this._position >= this._collection.GetCollection().Count;
        }
        public void MoveNext()
        {
           int index = this._collection.GetCollection().Skip(this._position + 1).TakeWhile(tower => !(tower is AttackingTower)).Count();
           this._position += index + 1;         
        }
    }
}