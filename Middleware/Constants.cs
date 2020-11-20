public static class Constants
{
    public static class RoutingConstants
    {
        public const string WEBSOCKET_REQUEST_PATH = "/ws";
    }
    public static class JsonCommands
    {
        public static class ServerCommands
        {
            public const string MOVE_TO = "s:MoveTo";
            public const string UNDO_MOVE_TO = "s:UndoMoveTo";
            public const string INITIAL = "s:Initial";
            public const string CONN_ID = "s:ConnID";
            public const string POWER_UP = "s:PowerUp";
            public const string UPGRADE_TOWER = "s:UpgradeTower";

            public const string ARRIVED_TO = "s:ArrivedTo";
        }

        public static class ClientCommands
        {
            public const string ARRIVED_TO= "c:ArrivedTo";
            public const string MOVE_TO = "c:MoveTo";
            public const string POWER_UP = "c:PowerUpClicked";
            public const string UPGRADE_TOWER = "c:UpgradeTower";

            public const string ATTACKING_TOWER_POWER_UP = "AttackingTowerPowerUp";
            public const string REGENERATING_TOWER_POWER_UP = "RegeneratingTowerPowerUp";
            public const string TOWER_DEFENCE_POWER_UP = "TowerDefencePowerUp";
        }
    }
} 