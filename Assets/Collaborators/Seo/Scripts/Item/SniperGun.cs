using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperGun : Gun
{
    public override void Use()
    {
        print("Fire!!!");
        bullet.damage = damage;
        GenerateSound(audioClipFire);
        Destroy(gameObject);
    }
}
