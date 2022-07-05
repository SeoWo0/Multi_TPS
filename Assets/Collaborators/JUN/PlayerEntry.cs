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
    public Button playerChagneChar;

    //플레이어 오브젝트
    public GameObject player1;
    public GameObject player2;

    //플레이어 번호
    int playerIndex = 1;

    private int ownerId;
    private bool isPlayerReady;

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            playerReadyButton.gameObject.SetActive(false);
            playerChagneChar.gameObject.SetActive(false);
        }
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);

        //프로퍼티
        Hashtable props = new Hashtable() { { GameData.PLAYER_READY, isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        //캐릭터 프로퍼티
        props = new Hashtable() { { GameData.PLAYER_CHAR, playerIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (PhotonNetwork.IsMasterClient)
        {
            LobbyManager.instance.LocalPlayerPropertiesUpdated();
        }
    }
    public void OnChageCharButton()
    {
        if (playerIndex == 2)
            playerIndex = 0;
        ChangeModel(playerIndex);
        playerIndex++;

        Hashtable props = new Hashtable() { { GameData.PLAYER_CHAR, playerIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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

    //캐릭터 변경
    public void ChangeModel(int playerModel)
    {
        switch (playerModel)
        {
            case 0:
                player1.SetActive(true);
                player2.SetActive(false);
                break;
            case 1:
                player1.SetActive(false);
                player2.SetActive(true);
                break;
        }
    }
}
