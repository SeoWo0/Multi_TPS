using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundDistributor : MonoBehaviour
{
    public string[] soundGroupNames;
    public Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (var _groupName in soundGroupNames)
        {
            GameObject _soundGroupObj = new GameObject {name = _groupName, transform = {position = gameObject.transform.position}};

            AudioSource _addedSource = _soundGroupObj.AddComponent<AudioSource>();
            _addedSource.spatialBlend = 1;
            _addedSource.transform.parent = transform;

            SoundSynchronizer _soundSync = _soundGroupObj.AddComponent<SoundSynchronizer>();

            _soundSync.type = SoundType.Effect;
            _soundSync.source = _addedSource;

            audioSourceDic.Add(_groupName, _addedSource);
        }
    }
    
    public AudioClip PlaySound(List<SoundGroup> targetGroup, string soundName, int soundIndex, float volume = 1f, float pitch = 1f)
    {
        SoundGroup _group = targetGroup.Find(group => group.groupName == soundName);

        SoundManager.Instance.PlayAt(_group.audioClipList[soundIndex], audioSourceDic[_group.groupName], volume, pitch);

        return _group.audioClipList[soundIndex];
    }
}
