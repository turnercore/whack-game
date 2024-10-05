using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WackableButton : MonoBehaviour
{
    private bool isWackable = false;
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the EventBus, OnEnableWackableButtons event

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum ButtonTypes
{
    Play,
    Quit,
    Resume,
    Restart,
    MainMenu,
    Options,
    Credits
}