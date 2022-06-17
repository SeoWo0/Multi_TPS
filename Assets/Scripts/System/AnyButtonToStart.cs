using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyButtonToStart : MonoBehaviour
{
    public string selectScene;


    private void Update() {
        
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(selectScene);
        }
    }

}
