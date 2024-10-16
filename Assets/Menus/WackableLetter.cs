using UnityEngine;

public class WackableLetter : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Vector3 originalScale;
    private bool isWacked = false;

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
