using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Managers;

public class SpeedUp : Item
{
    Collider coll;

    public override void Use()
    {
        GameManager.Instance.player.MoveSpeed += 3;

        Invoke(nameof(RestoreBuff), 3.5f);

        photonView.RPC(nameof(DeActivate), RpcTarget.All);
    }

    public void RestoreBuff()
    {
        // speedUp 아이템 습득 후 죽을 시 MoveSpeed가 내려가는 것을 방지
        if (GameManager.Instance.player.MoveSpeed < 5) return;
        GameManager.Instance.player.MoveSpeed -= 3;

        photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
    }

    [PunRPC]
    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) { }
}