using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;
using WeirdUnitGame.GameLogic;

namespace WeirdUnitBE.GameLogic
{
    public interface IAggregator
    {
        IIterator GetAttackingIterator();
        IIterator GetRegeneratingIterator();
    }
}