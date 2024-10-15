using System.Collections;
using UnityEngine;

public class WackableButton : MonoBehaviour
{
    [SerializeField]
    private ButtonTypes buttonType;

    // This is a flag that determines if the button is interactable at all
    [SerializeField]
    private bool isInteractable = true;
    private bool isWacked = false;

    [SerializeField]
    private float wackedDelay = 1f;

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
        isWacked = true;
        yield return new WaitForSeconds(wackedDelay);
        Debug.Log("Button Wacked: " + buttonType);
        EventBus.Instance.TriggerButtonWacked(buttonType);
        isWacked = false;
    }
}
