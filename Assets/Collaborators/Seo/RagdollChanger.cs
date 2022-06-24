using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject charObj;
    [SerializeField]
    private GameObject ragdollObj;
    [SerializeField]
    private Collider frictionCollider;
    // [SerializeField]
    // private Rigidbody spine;

    public void ChangeRagdoll()
    {
        //frictionCollider.enabled = false;
        //GetComponent<Collider>().enabled = false;
        charObj.SetActive(false);
        ragdollObj.SetActive(true);
    }
}
