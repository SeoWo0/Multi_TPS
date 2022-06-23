using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SniperGun : Item
{
    public float maxRange;
    public int damage;

    public GameObject soundEffectPrefab;
    public AudioClip fireAudioClip;

    public override void Use()
    {
        Fire();
    }

    private void Fire()
    {
        print("Fire!!!");
        GameObject _soundObj = PhotonNetwork.Instantiate("@SoundEffect", transform.position, Quaternion.identity);
        //GameObject _soundObj = Instantiate(soundEffectPrefab, transform.position, Quaternion.identity);
        AudioSource _source = _soundObj.GetComponent<AudioSource>();
        SoundManager.Instance.PlayAt(fireAudioClip, _source);
        Destroy(gameObject);
    }
}
