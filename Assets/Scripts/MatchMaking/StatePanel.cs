using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StatePanel : MonoBehaviour
{
    [SerializeField]
    private Text stateText;

    private Photon.Realtime.ClientState state;
    
    void Update()
    {
        if (state == PhotonNetwork.NetworkClientState)
            return;

        state = PhotonNetwork.NetworkClientState;
        stateText.text = state.ToString();
        Debug.Log("PhotonNetwork State : " + state.ToString());
    }
}

