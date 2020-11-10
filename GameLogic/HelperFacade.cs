using System;
using System.Collections.Generic;

namespace WeirdUnitBE.GameLogic
{
    public abstract class HelperFacade
    {
        private Randomizer _randomizer;
        private ClassAnalyzer _analyzer;
        
        public HelperFacade(Randomizer randomizer, ClassAnalyzer classAnalyzer)
        {
            _analyzer = classAnalyzer;
            _randomizer = randomizer;
        }

        public int ReturnRandomInteger(int from, int to)
        {
            return _randomizer.ReturnRandomInteger(from, to);
        }

        public List<Type> GetAllLeafClasses(Type baseClass)
        {
            return _analyzer.GetAllLeafClasses(baseClass);
        } 
    }
}