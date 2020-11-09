
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using System.Linq;
namespace WeirdUnitBE.GameLogic.Services.Implementation
{

    class NeutralTowerDecorator : TowerDecorator
    {
        
        public NeutralTowerDecorator (Tower tower, Position position): base(tower)
        {
            _tower.position = position;
            _tower.unitCount = 10;
        }

    }


}