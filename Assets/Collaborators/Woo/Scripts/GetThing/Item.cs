﻿using System;
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

    public UnityAction onGetItemEvent;

    public abstract void Use();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        print("OnGetItem");
        photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
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
}