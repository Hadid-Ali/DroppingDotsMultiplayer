using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GameEvents
{
    public static class NetworkGameplayEvents
    {
    
        public static GameEvent OnGameStarted = new();
        public static GameEvent<Turn> OnGameWin = new();
        public static GameEvent OnGameReset = new();
    }
    
    public static class NetworkPlayerEvents
    {


    }
}
