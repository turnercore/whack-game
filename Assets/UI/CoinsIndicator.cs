using System.Collections;
using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    private TMP_Text TMPro;
    private string CoinsString => GameManager.Instance.GetCoins().ToString();
    // Start is called before the first frame update
    void Start()
    {
        TMPro = GetComponent<TMP_Text>();
        TMPro.text = CoinsString;
    }

    void LateUpdate()
    {
        TMPro.text = CoinsString;
    }
}
