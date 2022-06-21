using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Item
{
    [SerializeField]
    private Transform   muzzlePos;      // 발사 위치
    [SerializeField]
    private GameObject  bullet;         // 발사체
    

    [Header ("Audio Clip")]
    [SerializeField]
    private AudioClip   audioClipFire;  // 발사 사운드
    [SerializeField]
    private AudioClip   audioClipTake;  // 장착 사운드  
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
    }

    public void GetGun() 
    {
        // TODO :: 바닥의 총을 줏었을때
        audioSource.PlayOneShot(audioClipTake);
    }

    public override void Use()
    {
        Fire();
        Destroy(gameObject);
    }

    public void Fire()
    {   
        audioSource.PlayOneShot(audioClipFire);
        Instantiate(bullet, muzzlePos.position, Quaternion.identity);

    }

}
