using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public const int COUNTDOWN = 5;
    public const int SPAWNCOUNTDOWN = 3;

    public const string PLAYER_READY = "Ready";
    public const string PLAYER_LOAD = "Load";
    public const string PLAYER_START = "StartGame";

    public const string PLAYER_CHAR = "Character";

    public const string ROOM_CHAT_CHANNEL = "RoomChattingChannel";
    public const string ROOM_SET_MAP = "MapSetting";
    public const string ROOM_SET_MODE = "ModeSetting";
    public const string ROOM_SET_TIME_LIMIT = "TimeLimitSetting";
    
    public const string PLAYER_SCORE = "Score";
    public const string GAME_IS_COMPLETE = "GameComplete";
    
    public static Color GetColor(int playerNumber)
    {
        switch(playerNumber)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.magenta;
            case 6: return Color.white;
            case 7: return Color.black;
            default: return Color.grey;
        }
    }
}
