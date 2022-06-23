using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject charObj;
    [SerializeField]
    private GameObject ragdollObj;
    // [SerializeField]
    // private Rigidbody spine;

    public void ChangeRagdoll()
    {
        charObj.SetActive(false);
        ragdollObj.SetActive(true);
    }
}
