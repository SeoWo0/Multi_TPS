using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperGun : Item
{
    public float maxRange;
    public int damage;

    public override void Use()
    {
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }


}
