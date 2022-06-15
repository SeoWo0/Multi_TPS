using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance { get; private set;}
    
    [Header("Panel")]

    private void Awake() {
        instance = this;
    }
}
