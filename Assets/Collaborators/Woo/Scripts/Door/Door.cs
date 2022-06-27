using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Door : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.Translate(Vector3.back * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.Translate(Vector3.back * Time.deltaTime);
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            transform.Translate(Vector3.forward * Time.deltaTime);
        
    }
}
    
