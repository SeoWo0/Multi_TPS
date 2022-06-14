using UnityEngine;

public enum EGameMode
{
    DeathMatch,
    TeamDeathMatch,
    Tournament,
}

public enum EMapType
{
    Crossline,
}

[CreateAssetMenu(fileName = "RoomData", menuName = "Room/RoomSettingData")]
public class RoomSettingData : ScriptableObject
{
    [Header("맵")]
    public EMapType mapType;

    [Header("게임 모드")]
    public EGameMode gameMode;

    [Header("최대 플레이어 수")]
    public byte maxPlayerCount;

    [Header("제한 시간")]
    public float timeLimit; // Seconds
}
