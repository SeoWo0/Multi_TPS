using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShotGun : Gun
{
    public void GetGun() 
    {
        GenerateSound(audioClipTake);
    }

    public override void Use()
    {
        Fire();
    }

    public void Fire()
    {
        GenerateSound(audioClipFire);

        Destroy(gameObject);
    }
}
