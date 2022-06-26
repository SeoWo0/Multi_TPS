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
#if UNITY_EDITOR
        // 에디터 상에서의 게임 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 애플리케이션에서의 종료
        Application.Quit();
        //
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
}
