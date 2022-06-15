using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

public class PlayerEntry : MonoBehaviour
{
    [Header("UI References")]
    public Text playerNameText;
    public Button playerReadyButton;
    public Image playerReadyImage;

    private int ownerId;
    private bool isPlayerReady;

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
        }
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);

        Hashtable props = new Hashtable() { { GameData.PLAYER_READY, isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            LobbyManager.instance.LocalPlayerPropertiesUpdated();
        }
    }

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        playerNameText.text = playerName;
    }

    public void SetPlayerReady(bool playerReady)
    {
        playerReadyImage.color = playerReady ? Color.green : Color.red;
    }
}
