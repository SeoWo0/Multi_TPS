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
    public Transform[] shotPos;

    [Header("Audio Clip")]
    [SerializeField]
    protected AudioClip audioClipFire;  // 발사 사운드
    public GameObject soundEffectPrefab;

    [Header("Gun Fx")]
    public ProjectileMover bullet;

    public override void Use() { }
}
