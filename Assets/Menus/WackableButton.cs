using System.Collections;
using UnityEngine;

public class WackableButton : MonoBehaviour
{
    [SerializeField]
    private ButtonTypes buttonType;

    // This is a flag that determines if the button is interactable at all
    [SerializeField]
    private bool isInteractable = true;
    private bool isWackable = false;
    private bool isWacked = false;

    [SerializeField]
    private float wackedDelay = 1f;

    void Start()
    {
        // Subscribe to the EventBus OnEnableWackableButtons and OnDisable events
        EventBus.Instance.OnWackableButtonsEnabled += EnableWackable;
        EventBus.Instance.OnWackableButtonsDisabled += DisableWackable;
    }

    // Clean Up
    private void OnDestroy()
    {
        // Unsubscribe from the EventBus OnEnableWackableButtons and OnDisable events
        EventBus.Instance.OnWackableButtonsEnabled -= EnableWackable;
        EventBus.Instance.OnWackableButtonsDisabled -= DisableWackable;
    }

    private void EnableWackable()
    {
        isWackable = true;
    }

    private void DisableWackable()
    {
        isWackable = false;
    }

    // When the button gets wacked (collides with player weapon, aka weapon tag) it will trigger the event
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInteractable && !isWacked && collision.gameObject.CompareTag("Weapon"))
        {
            //Trigger on delay to allow for animation
            StartCoroutine(TriggerWackedEvent());
        }
    }

    private IEnumerator TriggerWackedEvent()
    {
        yield return new WaitForSeconds(wackedDelay);
        Debug.Log("Button Wacked: " + buttonType);
        EventBus.Instance.TriggerButtonWacked(buttonType);
        isWacked = true;
    }
}
