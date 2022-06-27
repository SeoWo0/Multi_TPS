using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitButton : MonoBehaviour
{
    [SerializeField]
    private GameObject targetWindow;

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            targetWindow.SetActive(false);
        }
    }
}
