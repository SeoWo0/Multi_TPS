using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Managers;

public class PlayerResultPanel : MonoBehaviour
{
    [Header("Player Info")]
    public string playerName;
    
    public GameObject[] playerCharacters;

    public int playerScore;
    public bool isWinner;

    [Header("UI Resources")]
    public TextMeshProUGUI nameText;
    public Transform characterHolder;
    public TextMeshProUGUI scoreText;
    public GameObject winnerCrown;

    [Header("Owner Info")]
    public int ownerNumber;

    //private GameObject m_myPlayer;

    private void Start()
    {
        if (ownerNumber != PhotonNetwork.LocalPlayer.GetPlayerNumber()) return;
        //if (!photonView.IsMine) return;
        ShowGameResult(PhotonNetwork.LocalPlayer.NickName, GameManager.Instance.myScore.score);
    }
    
    public void ShowGameResult(string nickName, int score)
    {
        // 플레이어 이름 설정
        playerName = nickName;

        // 플레이어 점수 설정
        playerScore = score;

        SetWinner();
        SetGameResult();
    }
    
    private void SetWinner()
    {
        // 점수 정렬하여 제일 높은 점수를 가진 로컬 플레이어 isWinner = true 설정
        List<int> _scoreList = new List<int>();
        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            object _score;
            if (_player.CustomProperties.TryGetValue(GameData.PLAYER_SCORE, out _score))
            {
                _scoreList.Add((int) _score);
            }
        }

        // 오름차순 정렬
        _scoreList.Sort();
        print("List Count" + _scoreList.Count);
        //print("Player Score : " + playerScore);
        //print("List Target Score : " + _scoreList[_scoreList.Count - 1]);

        if (playerScore == _scoreList[_scoreList.Count - 1])
        {
            isWinner = true;
        }
    }
    
    private void SetGameResult()
    {
        nameText.text = playerName;
        
        if (playerCharacters.Length > 0)
        {
            foreach (Player _player in PhotonNetwork.PlayerList)
            {
                if (ownerNumber != _player.GetPlayerNumber()) continue;
                
                _player.CustomProperties.TryGetValue(GameData.PLAYER_CHAR, out var _playerIndex);

                if (_playerIndex != null)
                {
                    GameObject _character = playerCharacters[(int)_playerIndex - 1];

                    // string _name = _character.name;
                    // if (_name.Contains("(Clone)"))
                    // {
                    //     _name = _name.Replace(" (Clone)", string.Empty);
                    // }

                    GameObject _myPlayer = Instantiate(_character, _character.transform.position, _character.transform.rotation);
                    _myPlayer.transform.SetParent(characterHolder);
                    _myPlayer.transform.SetPositionAndRotation(characterHolder.position, _character.transform.rotation);
                    _myPlayer.transform.localScale = _character.transform.localScale;
                }
            }
        }

        scoreText.text = playerScore.ToString();
        
        winnerCrown.SetActive(isWinner);
    }
    
    // [PunRPC]
    // public void SetParentOfPlayer()
    // {
    //     m_myPlayer.transform.SetParent(characterHolder);
    //     m_myPlayer.transform.SetPositionAndRotation(characterHolder.position, playerCharacter.transform.rotation);
    //     m_myPlayer.transform.localScale = playerCharacter.transform.localScale;
    // }
}