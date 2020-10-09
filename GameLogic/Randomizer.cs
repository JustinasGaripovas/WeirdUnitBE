using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic
{
    public static class Randomizer
    {
        private static int _seed = DateTime.Now.Millisecond;
        public static int ReturnRandomInteger(int from, int to)
        {
            Random random = new Random(_seed);
            _seed += 100;
            int rInteger = random.Next(from, to);
            return rInteger;
        }
        
        public static Type ReturnRandomType<T>()
        {
            Type type = typeof(T);
            List<Type> typeList = ClassAnalyzer.GetAllLeafClasses(type);
            int rInteger = ReturnRandomInteger(0, typeList.Count);
            return typeList[rInteger];
        }

    }
}