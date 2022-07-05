using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Managers;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    [Header("Images")]
    public Image ownerBorder;
    public Image ownerIcon;
    public Image scoreBoardBorder;

    [Header("Color Info")] 
    public Color ownerBorderColor;
    public Color ownerIconColor;
    
    [Header("Score Info")]
    public int score;
    public int ownerNumber;
    public string ownerName;
    //private WaitForSeconds m_waitSeconds;

    private void Start()
    {
        scoreBoardBorder.color = Color.white;
        
        //Debug.LogError($"LocalPlayerNumber : {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        //Debug.LogError($"owner : {ownerNumber}");
        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() != ownerNumber) return;

        ownerBorder.color = ownerBorderColor;
        ownerIcon.color = ownerIconColor;
        scoreBoardBorder.color = ownerBorderColor;
        
        //m_waitSeconds = new WaitForSeconds(ownerNumber + 0.5f);
        
        //StartCoroutine(UpdateScore(0));
        UpdateScore(0);
    }

    public void UpdateScore(int earnedScore)
    {
        score += earnedScore;
        
        if (GameManager.Instance.isGameCompleted) return;
        
        SetScore(score);
    }

    public void SetScore(int score)
    {
        // 점수 패널은 각각의 client마다 플레이어 수 및 PlayerNumber에 맞게 생성해주기 때문에
        // (ex. ClientCount == 4 -> MasterClient : 4 score panel, Client 1 : 4 score panel, ...)
        // OnPlayerPropertiesUpdate에서 property가 update된 플레이어의
        // 해당 score 또한 Update 시켜주어야 UpdateScore가 제대로 동작함
        this.score = score;
        scoreText.text = score.ToString();
    }
    
    // 테스트 버전 => 로컬 플레이어가 상대 플레이어를 죽였을 때 얻은 score만큼
    // Update해주도록 해야함
    // public IEnumerator UpdateScore(int score)
    // {
    //     this.score += score;
    //
    //     while (true)
    //     {
    //         this.score++;
    //
    //         SetScore(this.score);
    //
    //         if (GameManager.Instance.isGameCompleted) yield break;
    //         // Score 동기화
    //         Hashtable _prop = new Hashtable() { { GameData.PLAYER_SCORE, this.score } };
    //         PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    //         
    //         yield return m_waitSeconds;
    //     }
    // }
}
