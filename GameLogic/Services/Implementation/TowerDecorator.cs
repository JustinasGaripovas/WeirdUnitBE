using WeirdUnitBE.GameLogic.TowerPackage.Towers;


namespace WeirdUnitBE.GameLogic.Services.Implementation
{

    class TowerDecorator : Tower
    {
        protected Tower _tower;

        public TowerDecorator (Tower tower)
        {
            _tower = tower;
        }
    }

}