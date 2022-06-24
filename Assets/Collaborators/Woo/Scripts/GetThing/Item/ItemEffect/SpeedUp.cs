using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpeedUp : Item
{
    public override void Use()
    {
        GameManager.Instance.player.MoveSpeed += 3;
        Invoke(nameof(RestoreBuff), 3.5f);
    }

    public void RestoreBuff()
    {
        GameManager.Instance.player.MoveSpeed -= 3;

        photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
    }

    private void OnTriggerEnter(Collider other) { }
}