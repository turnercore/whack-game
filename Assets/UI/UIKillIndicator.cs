using System.Collections;
using TMPro;
using UnityEngine;

public class UIKillIndicator : MonoBehaviour
{
    private TMP_Text TMPro;
    // Start is called before the first frame update
    void Start()
    {
        TMPro = GetComponent<TMP_Text>();
        // Subscribe to Enemy Killed event
        EventBus.Instance.OnKillsTextUpdate += OnKillsTextUpdate;
        TMPro.text = GameManager.Instance.GetKills().ToString();

    }
    private void OnDestroy()
    {
        // Unsubscribe from the event
        EventBus.Instance.OnKillsTextUpdate -= OnKillsTextUpdate;
    }

    void OnKillsTextUpdate()
    {
        // Wait until the next frame and then update the text
        StartCoroutine(DelayedUpdateText(GameManager.Instance.GetKills().ToString()));
    }

    // Delayed update text
    IEnumerator DelayedUpdateText(string text)
    {
        yield return new WaitForEndOfFrame();
        TMPro.text = text;
    }
}
