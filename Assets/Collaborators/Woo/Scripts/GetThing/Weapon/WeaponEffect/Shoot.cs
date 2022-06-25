using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shoot : Item
{
    public override void Use()
    {
        photonView.RPC(nameof(GainItem), RpcTarget.MasterClient);
    }
}
