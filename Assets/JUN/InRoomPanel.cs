using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class InRoomPanel : MonoBehaviour
{
    public GameObject playerListContent;
    public Button startGameButton;
    public GameObject playerEntry;

    private Dictionary<int, GameObject> playerListEntries;

    //private void OnEnable()
    //{
    //    if(playerListEntries==null)
    //    {
    //        playerListEntries = new Dictionary<int, GameObject>();
    //    }

    //    foreach(Player p in PhotonNetwork.PlayerList)
    //    {
    //        GameObject entry = Instantiate(playerEntry);
    //        entry.transform.SetParent(playerListContent.transform);
    //        entry.transform.SetPositionAndRotation(playerListContent.transform.position, Quaternion.identity);
    //        entry.transform.localScale = Vector3.one;
    //        entry.GetComponent<PlayerEntry>();

    //        object isPlayerReady;
    //        if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
    //        {
    //            entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
    //        }

    //        playerListEntries.Add(p.ActorNumber, entry);
    //    }

    //    startGameButton.gameObject.SetActive(CheckPlayersReady());

    //    Hashtable props = new Hashtable
    //    {
    //        {GameData.PLAYER_LOAD, false}
    //    };
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    //}

    //private void OnDisable()
    //{
    //    foreach (GameObject entry in playerListEntries.Values)
    //    {
    //        Destroy(entry.gameObject);
    //    }

    //    playerListEntries.Clear();
    //    playerListEntries = null;
    //}

    //public void OnLeaveRoomClicked()
    //{
    //    PhotonNetwork.LeaveRoom();
    //}

    //public void OnStartGameButtonClicked()
    //{
    //    PhotonNetwork.CurrentRoom.IsOpen = false;
    //    PhotonNetwork.CurrentRoom.IsVisible = false;

    //    PhotonNetwork.LoadLevel("GameScene");
    //}

    //private bool CheckPlayersReady()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        return false;
    //    }

    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        object isPlayerReady;
    //        if (p.CustomProperties.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
    //        {
    //            if (!(bool)isPlayerReady)
    //            {
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    //public void LocalPlayerPropertiesUpdated()
    //{
    //    startGameButton.gameObject.SetActive(CheckPlayersReady());
    //}

    //public void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    GameObject entry = Instantiate(playerEntryPrefab);
    //    entry.transform.SetPositionAndRotation(playerListContent.transform.position, Quaternion.identity);
    //    entry.transform.SetParent(playerListContent.transform);
    //    entry.transform.localScale = Vector3.one;
    //    entry.GetComponent<PlayerEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

    //    playerListEntries.Add(newPlayer.ActorNumber, entry);

    //    startGameButton.gameObject.SetActive(CheckPlayersReady());
    //}

    //public void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
    //    playerListEntries.Remove(otherPlayer.ActorNumber);

    //    startGameButton.gameObject.SetActive(CheckPlayersReady());
    //}

    //public void OnMasterClientSwitched(Player newMasterClient)
    //{
    //    if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
    //    {
    //        startGameButton.gameObject.SetActive(CheckPlayersReady());
    //    }
    //}

    //public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
    //    if (playerListEntries == null)
    //    {
    //        playerListEntries = new Dictionary<int, GameObject>();
    //    }

    //    GameObject entry;
    //    if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
    //    {
    //        object isPlayerReady;
    //        if (changedProps.TryGetValue(GameData.PLAYER_READY, out isPlayerReady))
    //        {
    //            entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
    //        }

    //        object playerindex;
    //        if (changedProps.TryGetValue(GameData.PLAYER_CHAR, out playerindex))
    //        {
    //            entry.GetComponent<PlayerEntry>().ChangeModel((int)playerindex - 1);
    //        }
    //    }

    //    startGameButton.gameObject.SetActive(CheckPlayersReady());
    //}
}
