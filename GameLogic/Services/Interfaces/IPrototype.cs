using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Services.Interfaces
{
    public interface IPrototype
    {
         Tower Clone();
    }
}
