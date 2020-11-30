using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic
{
    public class GameStateFlyweightInfo
    {
        public (int X, int Y) mapDimensions;
        public double gameSpeed;

        public GameStateFlyweightInfo((int, int) mapDimensions, double gameSpeed)
        {
            this.mapDimensions = mapDimensions;
            this.gameSpeed = gameSpeed;
        }
    }
}