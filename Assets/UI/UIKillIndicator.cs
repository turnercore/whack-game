using TMPro;
using UnityEngine;

public class UIKillIndicator : MonoBehaviour
{
    private TMP_Text TMPro;
    private string KillsString => GameManager.Instance.GetKills().ToString();

    // Start is called before the first frame update
    void Start()
    {
        TMPro = GetComponent<TMP_Text>();
        // Subscribe to Enemy Killed event
        TMPro.text = KillsString;
    }

    void LateUpdate()
    {
        TMPro.text = KillsString;
    }
}
