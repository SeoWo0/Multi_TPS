using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpeedUp : Item
{
    public override void Use()
    {
        gameObject.SetActive(false);
        GameManager.Instance.player.MoveSpeed *= 2;
        Invoke(nameof(RestoreBuff), 3.5f);
    }

    private void RestoreBuff()
    {
        GameManager.Instance.player.MoveSpeed /= 2;

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) { }
}