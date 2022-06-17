using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using Managers;

public class GameManager : Singleton<GameManager>
{
    [Header("Start Info")]
    public TextMeshProUGUI infoText;
    //public Transform[] spawnPos;

    public GameObject playerPrefab;

    [Header("Score Info")]
    public Score playerScore;
    public GameObject scorePanel;
    public List<Score> managedScoreList;
    private Score m_myScore;

    #region UNITY

    private void Start()
    {
        // 로딩이 완료되서 Start()가 호출되면 로딩이 완료됐다는 커스텀 프로퍼티 설정해줌
        Hashtable _props = new Hashtable() { { GameData.PLAYER_LOAD, true } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_props);

        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            Score _obj = Instantiate(playerScore, Vector3.zero, Quaternion.identity);
            _obj.transform.SetParent(scorePanel.transform);
            _obj.transform.localScale = Vector3.one;
            _obj.ownerNumber = _player.GetPlayerNumber();

            m_myScore = _obj.ownerNumber == PhotonNetwork.LocalPlayer.GetPlayerNumber() ? _obj : null;

            managedScoreList.Add(_obj);
        }
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

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 나간 플레이어 Score 없애줌
        Destroy(m_myScore.gameObject);
        managedScoreList.Remove(m_myScore);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        object _score;
        if (changedProps.TryGetValue(GameData.PLAYER_SCORE, out _score))
        {
            m_myScore.SetScore((int)_score);
        }

        if (changedProps.ContainsKey(GameData.PLAYER_LOAD))
        {
            if (CheckAllPlayersLoadLevel())
            {
                StartCoroutine(StartCountdown());
            }
            else
            {
                // 나머지 플레이어 기다리기
                PrintInfo($"Wait for Players - {CheckLoadedPlayersCount()} / {PhotonNetwork.PlayerList.Length}");
            }
        }
    }

    #endregion PHOTON CALLBACK

    private IEnumerator StartCountdown()
    {
        PrintInfo("All Player Loaded!\nStart Count Down");
        yield return new WaitForSeconds(1f);

        for (int i = GameData.COUNTDOWN; i > 0; --i)
        {
            PrintInfo($"Count Down\n{i}");
            yield return new WaitForSeconds(1f);
        }

        PrintInfo("Start Game!");

        // 캐릭터 생성
        // int _playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        // PhotonNetwork.Instantiate("PlayerModel", spawnPos[_playerNumber].position, spawnPos[_playerNumber].rotation, 0);

        yield return new WaitForSeconds(1f);
        infoText.transform.parent.gameObject.SetActive(false);
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

    private void PrintInfo(string info)
    {
        Debug.Log(info);
        infoText.text = info;
    }
}
