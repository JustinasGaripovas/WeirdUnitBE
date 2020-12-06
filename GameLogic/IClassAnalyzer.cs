using System;
using System.Collections.Generic;
using System.Text;

namespace WeirdUnitGame.GameLogic
{
    public interface IClassAnalyzer
    {
        public List<Type> GetAllLeafClasses(Type baseClass);
    }
}
