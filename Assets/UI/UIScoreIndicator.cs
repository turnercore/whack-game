using System.Collections;
using TMPro;
using UnityEngine;

public class UIScoreIndicator : MonoBehaviour
{
    private TMP_Text TMPro;
    private int currentScore;
    private Coroutine scoreCoroutine;

    [SerializeField]
    private float updateDuration = 1.0f;

    private void Awake()
    {
        TMPro = GetComponent<TMP_Text>();
        TMPro.text = currentScore.ToString();
        // Subscribe to Score Changed event
    }

    private void Start()
    {
        EventBus.Instance.OnScoreChanged += UpdateScore;
    }

    private void OnDestroy()
    {
        // Unsubscribe from Score Changed event
        EventBus.Instance.OnScoreChanged -= UpdateScore;
    }

    // Update is called once per frame
    void UpdateScore(int newScore)
    {
        int targetScore = newScore;
        if (targetScore != currentScore)
        {
            if (scoreCoroutine != null)
            {
                StopCoroutine(scoreCoroutine);
            }
            scoreCoroutine = StartCoroutine(UpdateScoreAnimation(targetScore));
        }
    }

    private IEnumerator UpdateScoreAnimation(int targetScore)
    {
        int startScore = currentScore;
        float elapsedTime = 0f;

        while (elapsedTime < updateDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / updateDuration);
            currentScore = (int)Mathf.Lerp(startScore, targetScore, t);
            TMPro.text = currentScore.ToString();
            yield return null;
        }

        currentScore = targetScore;
        TMPro.text = currentScore.ToString();
    }
}
