using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedUp : Item
{
    public override void Use()
    {
        if (!photonView.IsMine)
            return;

        gameObject.SetActive(false);
        GameManager.Instance.player.MoveSpeed = 15f;

        Invoke("RestoreBuff", 3f);
    }

    private void RestoreBuff()
    {
        GameManager.Instance.player.MoveSpeed = 10f;
    }
}