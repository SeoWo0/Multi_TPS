using UnityEngine;

[CreateAssetMenu(fileName = "SoundOption", menuName = "Option/Sound Data")]
public class SoundOptionData : ScriptableObject
{
    [Range(0, 100)] public float volume_BGM;
    [Range(0, 100)] public float volume_Effect;
}
