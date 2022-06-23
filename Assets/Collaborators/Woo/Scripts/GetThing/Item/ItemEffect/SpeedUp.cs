using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedUp : Item
{
    public override void Use()
    {
        gameObject.SetActive(false);
        GameManager.Instance.player.MoveSpeed = GameManager.Instance.player.MoveSpeed + 3f;

        Invoke("RestoreBuff", 3f);
    }

    private void RestoreBuff()
    {
        GameManager.Instance.player.MoveSpeed = GameManager.Instance.player.MoveSpeed - 3f;

        photonView.RPC(nameof(OnGetItem), RpcTarget.MasterClient);
    }
}