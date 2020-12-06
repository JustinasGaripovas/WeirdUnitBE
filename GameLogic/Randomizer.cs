using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage;
using WeirdUnitGame.GameLogic;

namespace WeirdUnitBE.GameLogic
{
    public class Randomizer : IRandomizer
    {
        private int _seed = DateTime.Now.Millisecond;
        public int ReturnRandomInteger(int from, int to, int? seed = null)
        {
            Random random = new Random(seed??_seed);
            _seed += 100;
            int rInteger = random.Next(from, to);
            return rInteger;
        }
    }
}