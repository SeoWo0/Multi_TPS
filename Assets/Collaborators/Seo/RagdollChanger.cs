using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject charObj;
    [SerializeField]
    private GameObject ragdollObj;

    public void ChangeRagdoll()
    {
        charObj.SetActive(false);
        ragdollObj.SetActive(true);
    }
}
