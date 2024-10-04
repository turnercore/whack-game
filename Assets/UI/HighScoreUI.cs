using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    void Start()
    {
        scoreText.text = "";
        // Subscribe to Game Over Event
        GameManager.Instance.OnGameOver += DisplayScores;

    }

    // Clean up
    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= DisplayScores;
    }

    void DisplayScores() {
        string highScore = GameManager.Instance.GetHighScore().ToString();
        string playerScore = GameManager.Instance.Score.ToString();
        Debug.Log($"High Score: {highScore}\nYour Score: {playerScore}");
        scoreText.text = $"High Score: {highScore}\nYour Score: {playerScore}";
    }
}
