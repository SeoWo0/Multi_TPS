using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake() {
        
        Cursor.visible      = false;    // 마우스 커서 지우기
        Cursor.lockState    = CursorLockMode.Locked;    // 마우스 위치 Lock

    }


}
