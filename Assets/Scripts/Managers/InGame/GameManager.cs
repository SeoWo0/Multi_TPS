using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Managers
{
    public class GameManager : Singleton<GameManager>, IOnEventCallback
    {
        [Header("Start Info")] public TextMeshProUGUI infoText;
        //public Transform[] spawnPos;

        public Camera mapCamera;
        public Transform[] spawnPos;
        public PlayerMove player;

        [Header("Score Info")] public Score scorePrefab;
        public Score myScore;
        public GameObject scorePanel;
        public List<Score> managedScoreList;

        [Header("Timer")] public Timer gameTimer;

        [Header("Game Result")] public Transform gameResultPanel;
        public PlayerResultPanel playerResultPrefab;
        public List<PlayerResultPanel> managedResultList;
        public bool isGameCompleted;

        public GameObject crossHair;
        
        // Events
        public UnityAction onGameComplete;

        #region UNITY

        private void Start()
        {
            // 로딩이 완료되서 Start()가 호출되면 로딩이 완료됐다는 커스텀 프로퍼티 설정해줌
            Hashtable _props = new Hashtable() {{GameData.PLAYER_LOAD, true}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(_props);

            foreach (Player _player in PhotonNetwork.PlayerList)
            {
                Score _obj = Instantiate(scorePrefab, Vector3.zero, Quaternion.identity);
                _obj.transform.SetParent(scorePanel.transform);
                _obj.transform.localScale = Vector3.one;
                _obj.ownerNumber = _player.GetPlayerNumber();

                managedScoreList.Add(_obj);
            }

            myScore = GetMyScore();
        }

        #endregion UNITY

        #region PHOTON CALLBACK

        public override void OnDisconnected(DisconnectCause cause)
        {
            //Debug.Log($"Disconnected : {cause.ToString()}");
            SceneManager.LoadScene("LobbyScene");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            // 나간 플레이어 Score 없애줌
            foreach (Score _score in managedScoreList)
            {
                if (_score.ownerNumber != otherPlayer.GetPlayerNumber()) continue;

                managedScoreList.Remove(_score);
                Destroy(_score.gameObject);
                break;
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            object _scoreProp;
            if (changedProps.TryGetValue(GameData.PLAYER_SCORE, out _scoreProp))
            {
                Score _score = managedScoreList.Find(score => score.ownerNumber == targetPlayer.GetPlayerNumber());

                _score.SetScore((int) _scoreProp);
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

            if (changedProps.ContainsKey(GameData.GAME_IS_COMPLETE))
            {
                PlayerResultPanel _result =
                    managedResultList.Find(result => result.ownerNumber == targetPlayer.GetPlayerNumber());

                print(targetPlayer.NickName);
                if (_result)
                {
                    targetPlayer.CustomProperties.TryGetValue(GameData.PLAYER_SCORE, out _scoreProp);
                    if (_scoreProp != null)
                    {
                        print((int) _scoreProp);
                        _result.ShowGameResult(targetPlayer.NickName, (int) _scoreProp);
                    }
                }
            }
        }

        #endregion PHOTON CALLBACK

        private IEnumerator StartCountdown()
        {
            crossHair.gameObject.SetActive(false);
            scorePanel.gameObject.SetActive(false);

            PrintInfo("All Player Loaded!\nStart Count Down");
            yield return new WaitForSeconds(1f);

            for (int i = GameData.COUNTDOWN; i > 0; --i)
            {
                PrintInfo($"Count Down\n{i}");
                yield return new WaitForSeconds(1f);
            }

            PrintInfo("Start Game!");
            gameTimer.gameObject.SetActive(true);
            gameTimer.SetTimerState(true);

            // 캐릭터 생성
            // int _playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            // PhotonNetwork.Instantiate("PlayerModel", spawnPos[_playerNumber].position, spawnPos[_playerNumber].rotation, 0);

            mapCamera.gameObject.SetActive(false);
            PlayerSet();
            yield return new WaitForSeconds(1f);
            infoText.transform.parent.gameObject.SetActive(false);
        }
        
        public void StartRespawn()
        {
            StartCoroutine(nameof(PlayerSpawn));
        }

        IEnumerator PlayerSpawn()
        {
            yield return new WaitForSeconds(5f);
            PhotonNetwork.Destroy(player.gameObject);
            PlayerSet();
        }

        private void PlayerSet()
        {
            crossHair.gameObject.SetActive(true);
            scorePanel.gameObject.SetActive(true);

            int _playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();

            object _playerIndex;
            Player _p = PhotonNetwork.LocalPlayer;
            _p.CustomProperties.TryGetValue(GameData.PLAYER_CHAR, out _playerIndex);
            switch ((int)_playerIndex - 1)
            {
                case 0:
                    GameObject _playerModel = PhotonNetwork.Instantiate("Player 1", spawnPos[_playerNumber].position, spawnPos[_playerNumber].rotation, 0);
                    player = _playerModel.GetComponent<PlayerMove>();

                    break;
                case 1:
                    GameObject _playerModel2 =PhotonNetwork.Instantiate("Player", spawnPos[_playerNumber].position, spawnPos[_playerNumber].rotation, 0);
                    player = _playerModel2.GetComponent<PlayerMove>();
                    break;
            }

            player.onDeadEvent += StartRespawn;
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
                    if ((bool) _isPlayerLoaded)
                    {
                        _count++;
                    }
                }
            }

            return _count;
        }

        private void CompleteGameRound()
        {
            Debug.Log("GameComplete");
            isGameCompleted = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            onGameComplete?.Invoke();

            gameTimer.gameObject.SetActive(false);
            scorePanel.gameObject.SetActive(false);
            crossHair.gameObject.SetActive(false);
            gameResultPanel.parent.gameObject.SetActive(true);

            foreach (Player _player in PhotonNetwork.PlayerList)
            {
                PlayerResultPanel _resultObj = Instantiate(playerResultPrefab, Vector3.zero, Quaternion.identity);
                _resultObj.transform.SetPositionAndRotation(gameResultPanel.position, Quaternion.identity);
                _resultObj.transform.SetParent(gameResultPanel);
                // Screen Space Canvas에 생성될 것이므로 위치 재지정
                _resultObj.transform.localScale = Vector3.one;
                _resultObj.ownerNumber = _player.GetPlayerNumber();

                managedResultList.Add(_resultObj);
            }

            Hashtable _prop = new Hashtable() {{GameData.GAME_IS_COMPLETE, isGameCompleted}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
        }

        private void PrintInfo(string info)
        {
            Debug.Log(info);
            infoText.text = info;
        }

        private Score GetMyScore()
        {
            foreach (Score _score in managedScoreList)
            {
                if (_score.ownerNumber != PhotonNetwork.LocalPlayer.GetPlayerNumber()) continue;

                return _score;
            }

            return null;
        }

        private PlayerResultPanel GetMyResult()
        {
            foreach (PlayerResultPanel _result in managedResultList)
            {
                if (_result.ownerNumber != PhotonNetwork.LocalPlayer.GetPlayerNumber()) continue;

                return _result;
            }

            return null;
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == Timer.ON_TIMER_DONE_EVENT)
            {
                CompleteGameRound();
            }
        }

        public void OnReturnToRoom()
        {
            PhotonNetwork.LoadLevel("LobbyScene");
            
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }
    }
}