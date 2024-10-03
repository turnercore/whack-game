using System.Collections;
using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    private TMP_Text TMPro;
    // Start is called before the first frame update
    void Start()
    {
        TMPro = GetComponent<TMP_Text>();
        // Subscribe to Enemy Killed event
        EventBus.Instance.OnEnemyDied += OnEnemyDied;
        TMPro.text = GameManager.Instance.GetCoins().ToString();

    }

    void OnEnemyDied(Enemy enemy)
    {
        // Wait until the next frame and then update the text
        StartCoroutine(DelayedUpdateText(GameManager.Instance.GetCoins().ToString()));
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event
        EventBus.Instance.OnEnemyDied -= OnEnemyDied;
    }

    // Delayed update text
    IEnumerator DelayedUpdateText(string text)
    {
        yield return null;
        TMPro.text = text;
    }
}
