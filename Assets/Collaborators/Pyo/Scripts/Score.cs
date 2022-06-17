using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
    public int ownerNumber;

    public void UpdateScore(int score)
    {
        this.score += score;
        this.score++;

        SetScore(this.score);

        // Score 동기화
        Hashtable _prop = new Hashtable() { { GameData.PLAYER_SCORE, this.score } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(_prop);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
