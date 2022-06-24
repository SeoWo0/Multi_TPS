using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LoginPanel : MonoBehaviour
{
    [SerializeField]
    private InputField      playerNameInput;

    private void Start() {
        playerNameInput.text = "플레이어 " + Random.Range(1000, 10000);
    }

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        if(playerName == "")
        {
            LobbyManager.instance.ShowError("Invailed Player Name");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
