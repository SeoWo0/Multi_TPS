using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using Managers;

public class GameManager : Singleton<GameManager>
{
    // public Text infoText;
    public Transform[] spawnPos;

    //public GameObject playerPrefab;

    public PlayerMove player;

    #region UNITY

    private void Start()
    {
        // 로딩이 완료되서 Start()가 호출되면 로딩이 완료됐다는 커스텀 프로퍼티 설정해줌
        Hashtable _props = new Hashtable() { { GameData.PLAYER_LOAD, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_props);
    }

    #endregion UNITY

    #region PHOTON CALLBACK
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause.ToString()}");
        SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey(GameData.PLAYER_LOAD)) return;

        if (CheckAllPlayersLoadLevel())
        {
            StartCoroutine(StartCountdown());
        }
        else
        {
            // 나머지 플레이어 기다리기
            // PrintInfo($"Wait for Players - {CheckLoadedPlayersCount()} / {PhotonNetwork.PlayerList.Length}");
        }
    }

    #endregion PHOTON CALLBACK

    private IEnumerator StartCountdown()
    {
        //PrintInfo("All Player Loaded!\nStart Count Down");
        //yield return new WaitForSeconds(1f);

        //for (int i = GameData.COUNTDOWN; i > 0; --i)
        //{
        //    PrintInfo($"Count Down\n{i}");
        //    yield return new WaitForSeconds(1f);
        //}

        //PrintInfo("Start Game!");

        // 캐릭터 생성
        //int _playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        //PhotonNetwork.Instantiate("PlayerModel", spawnPos[_playerNumber].position, spawnPos[_playerNumber].rotation, 0);

        //yield return new WaitForSeconds(1f);
        //infoText.gameObject.SetActive(false);
        PlayerSet();
        yield break;
    }
    private void PlayerSet()
    {

        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        object playerIndex;
        Player p = PhotonNetwork.LocalPlayer;
        p.CustomProperties.TryGetValue(GameData.PLAYER_CHAR, out playerIndex);
        switch ((int)playerIndex - 1)
        {
            case 0:
                GameObject playerModel = PhotonNetwork.Instantiate("MisakiPlayer", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
                player = playerModel.GetComponent<PlayerMove>();
                
                break;
            case 1:
                GameObject playerModel2 =PhotonNetwork.Instantiate("Player", spawnPos[playerNumber].position, spawnPos[playerNumber].rotation, 0);
                player = playerModel2.GetComponent<PlayerMove>();
                break;
                
        }
    }
        private bool CheckAllPlayersLoadLevel()
    {
        return CheckLoadedPlayersCount() == PhotonNetwork.PlayerList.Length;
    }

    private int CheckLoadedPlayersCount()
    {
        int _count = 0;

        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            object _isPlayerLoaded;
            if (_player.CustomProperties.TryGetValue(GameData.PLAYER_LOAD, out _isPlayerLoaded))
            {
                if ((bool)_isPlayerLoaded)
                {
                    _count++;
                }
            }
        }

        return _count;
    }

    public void PrintInfo(string info)
    {
        Debug.Log(info);
        //infoText.text = info;
    }
}
