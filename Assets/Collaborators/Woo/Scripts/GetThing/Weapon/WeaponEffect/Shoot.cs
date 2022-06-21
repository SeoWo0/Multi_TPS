using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shoot : Item
{
    public override void Use()
    {
        if (!photonView.IsMine)
            return;

        PhotonNetwork.Destroy(gameObject);
    }
}
