using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
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
