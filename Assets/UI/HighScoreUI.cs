using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highScoreText;

    [SerializeField]
    private TMP_Text yourScoreText;

    void OnEnable()
    {
        DisplayScores();
    }

    void DisplayScores()
    {
        string highScore = GameManager.Instance.GetHighScore().ToString();
        string playerScore = GameManager.Instance.Score.ToString();
        highScoreText.text = $"High Score: {highScore}";
        yourScoreText.text = $"Your Score: {playerScore}";
    }
}
