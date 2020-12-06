using System;
using System.Collections.Generic;
using WeirdUnitGame.GameLogic;

namespace WeirdUnitBE.GameLogic
{
    public class HelperFacade
    {
        private IRandomizer _randomizer;
        private WeirdUnitGame.GameLogic.IClassAnalyzer _analyzer;
        
        public HelperFacade(IRandomizer randomizer, WeirdUnitGame.GameLogic.IClassAnalyzer classAnalyzer)
        {
            _analyzer = classAnalyzer;
            _randomizer = randomizer;
        }

        public int ReturnRandomInteger(int from, int to)
        {
            return _randomizer.ReturnRandomInteger(from, to, null);
        }

        public List<Type> GetAllLeafClasses(Type baseClass)
        {
            return _analyzer.GetAllLeafClasses(baseClass);
        } 
    }
}