using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreController : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI highScore;

    public void Start()
    {
        int playerScore = PlayerPrefs.GetInt("Gamescore");
        score.text = playerScore + "";

        int highestScore = PlayerPrefs.GetInt("Highscore");
        if (playerScore > highestScore)
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
            highScore.text = playerScore + "";
        } else
        {
            highScore.text = highestScore + "";
        }
    }
}
