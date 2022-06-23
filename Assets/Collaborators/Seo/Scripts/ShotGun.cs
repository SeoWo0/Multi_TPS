using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShotGun : Item
{
    [Header ("Gun Spec")]
    [SerializeField]
    private Transform   muzzlePos;      // 발사 위치
    public float maxRange = 10f;
    public int damage = 1;

    public List<SoundGroup> soundGroupList;
    public SoundDistributor soundDistributor;

    private void Awake()
    {
        if (soundDistributor == null)
            soundDistributor = GetComponentInChildren<SoundDistributor>();
    }

    private void Fire()
    {
        AudioClip _clip = soundDistributor.PlaySound(soundGroupList, "Fire", 0);
        Destroy(gameObject, _clip.length);
    }

    public override void Use()
    {
        Fire();
        //PhotonNetwork.Destroy(gameObject);
    }
}
