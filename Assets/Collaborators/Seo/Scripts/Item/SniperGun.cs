using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperGun : Item
{
    public float maxRange;
    public int damage;

    public List<SoundGroup> soundGroupList;
    public SoundDistributor soundDistributor;

    private void Awake()
    {
        if (soundDistributor == null)
            soundDistributor = GetComponentInChildren<SoundDistributor>();
    }

    public override void Use()
    {
        Fire();
        //PhotonNetwork.Destroy(gameObject);
    }

    private void Fire()
    {
        AudioClip _clip = soundDistributor.PlaySound(soundGroupList, "Fire", 0);
        Destroy(gameObject, _clip.length);
    }
}
