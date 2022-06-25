using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Gun : Item
{
    [Header("Gun Spec")]
    public Transform muzzlePos;      // 발사 위치
    public float maxRange;
    public int damage;

    [Header("Audio Clip")]
    [SerializeField]
    protected AudioClip audioClipFire;  // 발사 사운드
    [SerializeField]
    protected AudioClip audioClipTake;  // 장착 사운드
    public GameObject soundEffectPrefab;

    [Header("Gun Fx")]
    public ProjectileMover bullet;
    public GameObject fireFlameFx;
    public GameObject hitFx;

    public override void Use() { }

    public void GenerateSound(AudioClip clip)
    {
        GameObject _soundObj = PhotonNetwork.Instantiate("@SoundEffect", transform.position, Quaternion.identity);

        //GameObject _soundObj = Instantiate(soundEffectPrefab, transform.position, Quaternion.identity);
        AudioSource _source = _soundObj.GetComponent<AudioSource>();
        SoundManager.Instance.PlayAt(clip, _source);
    }

    private void OnTriggerEnter(Collider other) { }
}
