using System;
using System.Reflection;
using System.Linq;
using WeirdUnitBE.GameLogic.TowerPackage;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators;

namespace WeirdUnitBE.GameLogic.Services.Interfaces
{
    public interface IGameStateGenerator
    {
        GameState GenerateRandomGameState();
    }
}