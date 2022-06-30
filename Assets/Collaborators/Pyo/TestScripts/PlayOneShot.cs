using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayOneShot : MonoBehaviourPun
{
    private AudioSource m_source;

    private void Awake()
    {
        m_source = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        OneShot();
    }
    
    public void OneShot()
    {
        if (!m_source.clip) return;
        StartCoroutine(WaitForLateClip());
    }

    private IEnumerator WaitForLateClip()
    {
        yield return new WaitUntil(() => m_source.clip);
        yield return new WaitForSeconds(m_source.clip.length);
        
        PhotonNetwork.Destroy(gameObject);
    }
}
