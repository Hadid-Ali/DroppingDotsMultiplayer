
using System.Collections.Generic;

public static partial class GameEvents
{
    public static class MenuEvents
    {
        public static GameEvent<string, float> TimeBasedActionRequested = new();
        public static GameEvent<MenuName> MenuTransitionEvent = new();
        
        public static GameEvent<List<string>> PlayersListUpdated = new();
        public static GameEvent<List<string>> RoomsListUpdated = new();
        public static GameEvent MatchStartRequested = new();
        public static GameEvent<string> RoomJoinRequested = new();

        public static GameEvent<bool> OnMenuEnable = new();
    }
}
