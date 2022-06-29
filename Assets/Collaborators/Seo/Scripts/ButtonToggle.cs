using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    public GameObject targetObj;

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel") && targetObj.activeSelf)
        {
            targetObj.SetActive(false);
        }
    }

    public void ObjectToggle()
    {
        targetObj.SetActive(!targetObj.activeSelf);
    }
}
