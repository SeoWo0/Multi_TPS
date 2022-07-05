using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class InRoomPanel : MonoBehaviour
{
    public GameObject playerListContent;
    public Button startGameButton;
    public GameObject playerEntryPrefab;

    public RoomSettingPanel roomSettingPanel;

    private Dictionary<int, GameObject> playerListEntries;

    private void OnEnable()
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        Hashtable _props = new Hashtable() { { GameData.PLAYER_READY, false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_props);

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playerEntryPrefab);
            entry.transform.SetPositionAndRotation(playerListContent.transform.position, Quaternion.identity);
            entry.transform.SetParent(playerListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
            }
            
            object playerindex;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_CHAR, out playerindex))
            {
                entry.GetComponent<PlayerEntry>().ChangeModel((int)playerindex - 1);
            }
            playerListEntries.Add(p.ActorNumber, entry);
        }

        startGameButton.gameObject.SetActive(CheckPlayersReady());
        roomSettingPanel.CheckMasterClient();

        Hashtable props = new Hashtable
        {
            {GameData.PLAYER_LOAD, false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private void OnDisable()
    {
        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom(false);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        Hashtable _prop = new Hashtable() { { GameData.PLAYER_START, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);

        // 게임 시작 시 모든 플레이어의 ready custom property를 false로 돌려주기
        // -> 게임이 끝나고 룸으로 돌아왔을 때 모두가 다 room에 복귀해서 다시 ready를 눌러주기 위함
        _prop = new Hashtable() {{GameData.PLAYER_READY, false}};
        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            _player.SetCustomProperties(_prop);
        }
        
        PhotonNetwork.LoadLevel(roomSettingPanel.mapSelectText.text);
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void LocalPlayerPropertiesUpdated()
    {
        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void OnLeftRoom()
    {
        // RoomSettingPanel
        Hashtable _prop = new Hashtable() { { GameData.ROOM_SET_MAP, 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
        
        _prop = new Hashtable() { { GameData.ROOM_SET_MODE, 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
        
        _prop = new Hashtable() { { GameData.ROOM_SET_TIME_LIMIT, 0 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(playerEntryPrefab);
        entry.transform.SetPositionAndRotation(playerListContent.transform.position, Quaternion.identity);
        entry.transform.SetParent(playerListContent.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            startGameButton.gameObject.SetActive(CheckPlayersReady());
            // RoomSettingPanel
            roomSettingPanel.CheckMasterClient();
        }
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
            }
            
            //CharacterSetting
            object playerindex;
            if (changedProps.TryGetValue(GameData.PLAYER_CHAR, out playerindex))
            {
                entry.GetComponent<PlayerEntry>().ChangeModel((int)playerindex - 1);
            }
            
            if (changedProps.TryGetValue(GameData.PLAYER_START, out object _isStarted))
            {
                PhotonNetwork.LoadLevel(roomSettingPanel.mapSelectText.text);
            }
            
            // RoomSettingPanel
            if (changedProps.TryGetValue(GameData.ROOM_SET_MAP, out var _isPassedToNext))
            {
                roomSettingPanel.SetMapType((int)_isPassedToNext);
            }
            if (changedProps.TryGetValue(GameData.ROOM_SET_MODE, out _isPassedToNext))
            {
                roomSettingPanel.SetGameMode((int)_isPassedToNext);
            }
            if (changedProps.TryGetValue(GameData.ROOM_SET_TIME_LIMIT, out _isPassedToNext))
            {
                roomSettingPanel.SetTimeLimit((int)_isPassedToNext);
            }
        }

        startGameButton.gameObject.SetActive(CheckPlayersReady());
    }
}
