﻿using UnityEngine;
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

        if (useType == EUseType.Immediately)
        {
            photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void GainItem()
    {
        gameObject.SetActive(false);
        ItemSpawnManager.Instance.DestroyItemOnGain(gameObject);
        WeaponSpawnManager.Instance.DestroyItemOnGain(gameObject);
    }

    //[PunRPC]
    //public void OnGetItem()
    //{
    //    if (!PhotonNetwork.IsMasterClient) return;
        
    //    PhotonNetwork.Destroy(gameObject);
    //}

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (!other.CompareTag("Player")) return;
    //
    //     transform.GetComponent<Rotation>().enabled = false;
    //
    //     onGetItemEvent?.Invoke();
    //
    //     Destroy(gameObject);
    // }
}