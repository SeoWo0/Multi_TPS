using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject      roomContent;
    [SerializeField]
    private GameObject      roomEntryPrefab;

    private Dictionary<string, RoomInfo>    cachedRoomList;
    private Dictionary<string, GameObject>  roomListEntries;

    private void Start() {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    public void OnBackButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public void ClearRoomList()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(roomEntryPrefab);
            entry.transform.SetParent(roomContent.transform);
            entry.transform.SetPositionAndRotation(roomContent.transform.position,Quaternion.identity);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                    
                }
            }
            else if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }
}
