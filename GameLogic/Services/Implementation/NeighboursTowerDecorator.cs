using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.Map;
using System.Linq;

namespace WeirdUnitBE.GameLogic.Services.Implementation
{

    class NeighboursTowerDecorator : TowerDecorator
    {
            public NeighboursTowerDecorator (Tower tower): base(tower)
            {
                _tower.neighbourTowers = GetTowersFromPositions();
            }

            private List<Position> GetTowersFromPositions()
            {
                return MapService.GetDefaultMapConnections()[_tower.position].ToList();
            }        
    }



}