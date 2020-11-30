using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Services.Interfaces
{
    public interface IGameStateBuilder
    {
        GameStateFlyweightInfo GenerateFlyweightInfo((int, int) mapDimensions, double gameSpeed);
        ConcurrentDictionary<Position, Tower> GenerateTowers(string user1, string user2);
        List<PowerUp> GeneratePowerUps();
    }
}