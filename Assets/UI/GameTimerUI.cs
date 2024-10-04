using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    private TMP_Text timerText;
    private float timeElipsed => GameManager.Instance.timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer text to MM:SS format, always have 2 digits for minutes and seconds
        timerText.text = $"{(int)timeElipsed / 60:00}:{(int)timeElipsed % 60:00}";
        //timerText.text = $"{(int)timeElipsed / 60}:{(int)timeElipsed % 60}";
    }
}
