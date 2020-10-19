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
            public const string INITIAL = "s:Initial";
            public const string CONN_ID = "s:ConnID";
            public const string POWER_UP = "s:PowerUp";
        }

        public static class ClientCommands
        {
            public const string MOVE_TO = "c:MoveTo";
            public const string POWER_UP = "c:PowerUpClicked";

            public const string ATTACKING_TOWER_POWER_UP = "AttackingTowerPowerUp";
        }
    }
} 