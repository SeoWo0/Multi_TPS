using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Door : MonoBehaviour
{
    private Animator m_anim;

    private void Awake()
    {
        m_anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            m_anim.SetBool("IsOpen", true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            m_anim.SetBool("IsOpen", true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            m_anim.SetBool("IsOpen", false);
    }
}
    
