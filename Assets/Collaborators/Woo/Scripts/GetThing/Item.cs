﻿using Photon.Pun;
using UnityEngine;

public abstract class Item : MonoBehaviourPun
{
    public enum EUseType
    {
        Immediately,
        Has,

        Count
    }

    public enum EItemType
    {
        Weapon,
        Buff,
    }


    // 스폰된 지점 알기 위한 변수
    public int index;

    public EUseType useType;
    public EItemType itemType;

    public abstract void Use();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !photonView.IsMine) return;

        if (useType == EUseType.Immediately)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}