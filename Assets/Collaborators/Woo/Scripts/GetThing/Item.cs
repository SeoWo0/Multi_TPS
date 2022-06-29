using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public abstract class Item : MonoBehaviourPun
{
    public enum EUseType
    {
        Immediately,
        Has,
    }

    public enum EItemType
    {
        Weapon,
        Buff,
    }

    public enum EGunType
    {
        None,
        ShotGun,
        Sniper,
    }


    // 스폰된 지점 알기 위한 변수
    public int index;

    public EUseType useType;
    public EItemType itemType;
    public EGunType gunType;

    public AudioClip itemGetSfx;

    public UnityAction onGetItemEvent;

    public abstract void Use();
    public void PlayGetItemSfx(float volume = 1f)
    {
        GenerateSound(itemGetSfx);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        print("OnGetItem");
        photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
        PlayGetItemSfx();
    }

    [PunRPC]
    public void GainItem()
    {
        PhotonNetwork.Destroy(gameObject);
        
        switch (itemType)
        {
            case EItemType.Weapon:
                WeaponSpawnManager.Instance.CheckListRemove(index);
                break;
            case EItemType.Buff:
                ItemSpawnManager.Instance.CheckListRemove(index);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        print("OnGetItem");

    }

    public void GenerateSound(AudioClip clip, float volume = 1f)
    {
        GameObject _soundObj = PhotonNetwork.Instantiate("@SoundEffect", transform.position, Quaternion.identity);

        AudioSource _source = _soundObj.GetComponent<AudioSource>();
        SoundSynchronizer _soundSync = _soundObj.AddComponent<SoundSynchronizer>();
        _soundSync.type = SoundType.Effect;
        SoundManager.Instance.PlayAt(clip, _source);
    }
}