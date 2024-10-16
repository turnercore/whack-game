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
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Vector3 originalScale;

    private void Awake()
    {
        originalPosition = transform.position;
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
        isWacked = true;
        yield return new WaitForSeconds(wackedDelay);
        Debug.Log("Button Wacked: " + buttonType);
        EventBus.Instance.TriggerButtonWacked(buttonType);
        isWacked = false;
    }

    // Reset the button's state and original position when enabled
    private void OnEnable()
    {
        isWacked = false;
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        originalScale = transform.localScale;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        transform.position = originalPosition;
        transform.eulerAngles = originalRotation;
        transform.localScale = originalScale;
    }
}
