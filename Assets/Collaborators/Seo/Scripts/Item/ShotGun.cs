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
    
    [Header ("Audio Clip")]
    [SerializeField]
    private AudioClip   audioClipFire;  // 발사 사운드
    [SerializeField]
    private AudioClip   audioClipTake;  // 장착 사운드  
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void GetGun() 
    {
        // TODO :: 바닥의 총을 줏었을때
        audioSource.PlayOneShot(audioClipTake);
    }

    public override void Use()
    {
        // PhotonNetwork.Destroy(gameObject);
        Fire();
        Destroy(gameObject, audioClipFire.length);
    }

    public void Fire()
    {   
        audioSource.PlayOneShot(audioClipFire);
    }
}
