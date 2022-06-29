using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance { get; private set;}

    [Header("Panel")]
    public LoginPanel           loginPanel;
    public InConnectPanel       inConnectPanel;
    public CreateRoomPanel      createRoomPanel;
    public LobbyPanel           lobbyPanel;
    public InRoomPanel          inRoomPanel;
    public InfoPanel            infoPanel;

    [Header("Audios")]
    public AudioClip onConnectBGM;
    public AudioClip onClickButtonSfx;
    public AudioClip onHoverButtonSfx;
    public AudioClip onPlayerReadySfx;
    
    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            // return 했는데 만약 룸이 시작해버렸을 경우 Connect Panel로 이동
            if (!PhotonNetwork.CurrentRoom.IsOpen)
            {
                PhotonNetwork.LeaveRoom();
                SetActivePanel(PANEL.Connect);
                return;
            }

            SetActivePanel(PANEL.Room);
        }
        
        SoundManager.Instance.Play(onConnectBGM, SoundType.Bgm);
    }

    public enum PANEL {Login, Connect, Lobby, Room, CreateRoom}
    
    public void SetActivePanel(PANEL panel)
    {
        if (!loginPanel || !inConnectPanel || !lobbyPanel || !inRoomPanel || !createRoomPanel) return;
        
        loginPanel.gameObject.SetActive(panel == PANEL.Login);
        inConnectPanel.gameObject.SetActive(panel == PANEL.Connect);
        lobbyPanel.gameObject.SetActive(panel == PANEL.Lobby);
        inRoomPanel.gameObject.SetActive(panel == PANEL.Room);
        createRoomPanel.gameObject.SetActive(panel == PANEL.CreateRoom);
    }

    public void ShowError(string error)
    {
        infoPanel.ShowError(error);
    }

    #region PHOTON CALLBACKS

    public override void OnConnectedToMaster()
    {
        SetActivePanel(PANEL.Connect);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobbyPanel.OnRoomListUpdate(roomList);
    }

    public override void OnJoinedLobby()
    {
        lobbyPanel.ClearRoomList();
    }

    public override void OnLeftLobby()
    {
        lobbyPanel.ClearRoomList();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(PANEL.Connect);
        infoPanel.ShowError("Create Room Failed with Error(" + returnCode + ") : " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(PANEL.Connect);
        infoPanel.ShowError("Join Room Failed with Error(" + returnCode + ") : " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {  
        string roomName = "Room" + Random.Range(1000, 10000);
        // For Room Chatting Channel
        string _chattingChannel = "Channel " + Random.Range(0, 1000);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 4,
            CustomRoomProperties = new Hashtable { { GameData.ROOM_CHAT_CHANNEL, _chattingChannel } }
        };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(PANEL.Room);
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(PANEL.Connect);
        inRoomPanel.OnLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        inRoomPanel.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        inRoomPanel.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        inRoomPanel.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        inRoomPanel.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.TryGetValue(GameData.PLAYER_READY, out var _isPlayerReady))
        {
            if ((bool) _isPlayerReady)
            {
                OnPlayerReadyClicked();
            }
        }
    }

    public void LocalPlayerPropertiesUpdated()
    {
        inRoomPanel.LocalPlayerPropertiesUpdated();
    }

    #endregion PHOTON CALLBACKS

    #region AUDIOS
    public void PlayFxOnButtonClicked()
    {
        SoundManager.Instance.Play(onClickButtonSfx);
    }
    
    public void PlayFxOnButtonHover()
    {
        SoundManager.Instance.Play(onHoverButtonSfx);
    }

    public void OnPlayerReadyClicked()
    {
        SoundManager.Instance.Play(onPlayerReadySfx);
    }
    #endregion AUDIOS
}