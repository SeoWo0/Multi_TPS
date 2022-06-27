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
        //OneShot();
        photonView.RPC(nameof(OneShot), RpcTarget.All);
    }

    [PunRPC]
    public void OneShot()
    {
        if (!m_source) return;
        Destroy(gameObject, m_source.clip.length);
    }

    private IEnumerator WaitForLateClip()
    {
        yield return new WaitUntil(() => m_source.clip);
        
        Destroy(gameObject, m_source.clip.length);
    }
}
