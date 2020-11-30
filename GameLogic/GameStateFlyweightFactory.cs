using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic
{
    public static class GameStateFlyweightFactory
    {
        private static List<GameStateFlyweightInfo> flyweightInfoList = new List<GameStateFlyweightInfo>();

        public static GameStateFlyweightInfo GetflyweightInfo((int X, int Y) mapDimensions, double gameSpeed)
        {
            GameStateFlyweightInfo flyweightInfo = 
                flyweightInfoList
                .FirstOrDefault
                (
                    info => 
                        info.mapDimensions.Equals(mapDimensions) &&
                        info.gameSpeed == gameSpeed
                );

            if(flyweightInfo == null)
            {
                flyweightInfo = new GameStateFlyweightInfo(mapDimensions, gameSpeed);
                flyweightInfoList.Add(flyweightInfo);
            }

            return flyweightInfo;
        }
    }
}