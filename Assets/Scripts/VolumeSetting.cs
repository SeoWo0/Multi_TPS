using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider effectSlider;

    private void Update() {
        Debug.Log(SoundManager.Instance.soundOption.volume_BGM.ToString());    
    }

    private void Start() 
    {
        bgmSlider.value = SoundManager.Instance.soundOption.volume_BGM / 100;
        effectSlider.value = SoundManager.Instance.soundOption.volume_Effect / 100;

        // bgmSlider.onValueChanged.AddListener()

    }

    public void SoundBGMChange()
    {
        SoundManager.Instance.soundOption.volume_BGM = bgmSlider.value * 100;
    }
    
    public void SoundEffectChange()
    {
        SoundManager.Instance.soundOption.volume_Effect = effectSlider.value * 100;
    }
}
