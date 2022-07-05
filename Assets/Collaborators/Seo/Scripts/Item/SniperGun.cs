using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperGun : Gun
{
    public override void Use()
    {
        print("Fire!!!");
        GenerateSound(audioClipFire);
        gameObject.SetActive(false);
    }
}
