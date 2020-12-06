using System;
using System.Collections.Generic;
using System.Text;

namespace WeirdUnitGame.GameLogic
{
    public interface IRandomizer
    {
        public int ReturnRandomInteger(int from, int to, int? seed);
    }
}
