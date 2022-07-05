using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomPanel : MonoBehaviour
{
    [SerializeField]
    private InputField      createRoomNameInput;
    [SerializeField]
    private int             maxPlayerCount = 1;
    [SerializeField]
    private Text            playerText;
    
    public void OnCreateRoomCancleButtonClicked()
    {
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }               

    public void OnCreateRoomConfirmButtonClicked()
    {
        string roomName = createRoomNameInput.text;

        if(roomName == "")
        {
            roomName = PhotonNetwork.LocalPlayer.NickName + " 의 Room";
        }

        byte maxPlayer = (byte)maxPlayerCount;
        
        string _chattingChannel = "Channel " + Random.Range(0, 1000);
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer, 
            CustomRoomProperties = new Hashtable { { GameData.ROOM_CHAT_CHANNEL, _chattingChannel } } };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void MaxPlayerCountUpButtonClicked()
    {
        if(maxPlayerCount == 4)
            return;
        maxPlayerCount ++;
        playerText.text = maxPlayerCount.ToString();
    }
    
    public void MaxPlayerCountDownButtonClicked()
    {
        if(maxPlayerCount == 1)
            return;

        maxPlayerCount --;
        playerText.text = maxPlayerCount.ToString();
    }

}
