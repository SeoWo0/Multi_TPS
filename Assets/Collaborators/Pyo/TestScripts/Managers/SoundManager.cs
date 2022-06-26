using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Managers;

public enum SoundType
{
    Bgm,
    Effect,
    MaxCount,
}

[System.Serializable]
public class SoundGroup
{
    public string groupName;
    public List<AudioClip> audioClipList;
}

public class SoundManager : Singleton<SoundManager>
{
    public SoundOptionData soundOption;
    
    private AudioSource[] m_audioSources = new AudioSource[(int)SoundType.MaxCount];
    private Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    public float audioFadeTime;

    public UnityAction onSoundValueChangeEvent;

    private void Awake()
    {
        Init();
    }

    //private void OnEnable()
    //{
    //    SceneController.Instance.onSceneInEvent += Clear;

    //    SceneController.Instance.onSceneOutEvent -= ClearRegisteredEvents;
    //    SceneController.Instance.onSceneOutEvent += ClearRegisteredEvents;
    //}

    public void Init()
    {
        GameObject _root = GameObject.FindGameObjectWithTag("AudioRoot");

        if (_root) return;
        
        _root = new GameObject {name = "@Audio"};
        _root.tag = "AudioRoot";
        DontDestroyOnLoad(_root);

        string[] _soundNames = Enum.GetNames(typeof(SoundType));
        for (int i = 0; i < _soundNames.Length - 1; ++i)
        {
            GameObject _sounds = new GameObject { name = _soundNames[i] };

            m_audioSources[i] = _sounds.AddComponent<AudioSource>();
            SoundSynchronizer _soundSync = _sounds.AddComponent<SoundSynchronizer>();

            _soundSync.type = _soundNames[i] == "Bgm" ? SoundType.Bgm : SoundType.Effect;
            _soundSync.source = m_audioSources[i];

            _sounds.transform.parent = _root.transform;
        }

        // 배경음악은 무한 반복
        m_audioSources[(int)SoundType.Bgm].loop = true;
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑 및 음원 빼기
        foreach (AudioSource _audioSource in m_audioSources)
        {
            _audioSource.clip = null;
            _audioSource.Stop();
        }
        
        // 딕셔너리 비워주기
        m_audioClips.Clear();
    }

    public void Play(AudioClip audioClip, SoundType type = SoundType.Effect, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!audioClip) return;
        
        //StopAllCoroutines();
        
        if (type == SoundType.Bgm)
        {
            AudioSource _audioSource = m_audioSources[(int)SoundType.Bgm];
            if (_audioSource.isPlaying)
            {
                StartCoroutine(FadeAudio(_audioSource, 0));
                
                AudioSource _newSource = _audioSource.gameObject.AddComponent<AudioSource>();
                m_audioSources[(int) SoundType.Bgm] = _newSource;

                _newSource.volume = 0;
                _newSource.pitch = pitch;
                _newSource.clip = audioClip;
                _newSource.Play();
                
                StartCoroutine(FadeAudio(_newSource, volume * (soundOption.volume_BGM / 100)));
                return;
            }
            
            _audioSource.volume = 0;
            _audioSource.pitch = pitch;
            _audioSource.clip = audioClip;
            _audioSource.Play();
                
            StartCoroutine(FadeAudio(_audioSource, volume * (soundOption.volume_BGM / 100)));
        }
        else
        {
            AudioSource _audioSource = m_audioSources[(int)SoundType.Effect];
            _audioSource.volume = volume * (soundOption.volume_Effect / 100);
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(audioClip);
        }
    }

    public void PlayAt(AudioClip audioClip, AudioSource audioSource, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!audioClip) return;

        audioSource.clip = audioClip;
        audioSource.volume = volume * (soundOption.volume_Effect / 100);
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public void Play(string path, SoundType type = SoundType.Effect, float volume = 1f, float pitch = 1f)
    {
        AudioClip _audioClip = GetOrAddAudioClip(path, type);
        Play(_audioClip, type, volume, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, SoundType type = SoundType.Effect)
    {
        if (!path.Contains("Sounds/"))
        {
            path = $"Sounds/{path}";
        }

        AudioClip _audioClip = null;

        if (type == SoundType.Bgm)
        {
            _audioClip = Resources.Load<AudioClip>(path);
        }
        else // 효과음은 자주 사용되므로 Dictionary에 저장해두고 불러와서 씀
        {
            if (!m_audioClips.TryGetValue(path, out _audioClip))
            {
                _audioClip = Resources.Load<AudioClip>(path);
                m_audioClips.Add(path, _audioClip);
            }
        }

        // 오디오 클립을 못 찾았다면 로그 출력
        if (!_audioClip)
        {
            Debug.LogWarning($"Missing AudioClip! {path}");
        }

        return _audioClip;
    }

    public void OnSoundValueChange()
    {
        onSoundValueChangeEvent?.Invoke();
    }

    private void ClearRegisteredEvents()
    {
        print("SoundManager's Event Cleared!");
        onSoundValueChangeEvent = null;
    }
    
    IEnumerator FadeAudio (AudioSource channel, float targetVolume)
    {
        float _time = 0;
        float _start = channel.volume;

        while (_time < audioFadeTime)
        {
            _time += Time.deltaTime;
            channel.volume = Mathf.Lerp(_start, targetVolume, _time / audioFadeTime);
            yield return null;
        }
        
        channel.volume = targetVolume;
        if (targetVolume == 0)
        {
            Destroy(channel);
        }
    }
}
