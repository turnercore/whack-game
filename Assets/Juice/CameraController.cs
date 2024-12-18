using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public bool isFrozen = false;

    [SerializeField]
    private float shakeDuration = 0.5f;

    [SerializeField]
    private float shakeMagnitude = 0.5f;

    [SerializeField]
    private float floatyAmount = 0.02f;

    [SerializeField]
    private float floatySpeed = 0.2f;

    [SerializeField]
    private float zoomTransitionTime = 5f;

    [SerializeField]
    private float zoomedInSize = 3f;

    [SerializeField]
    private float zoomedOutSize = 7.2f;

    public bool isInMenu = false;

    private Coroutine transitionCoroutine;
    private Vector3 currentTarget = new(4f, 3.70000005f, 0f);
    private float zoomElapsedTime = 0f;
    public Vector3 menuPosition = new(0, 0, -10);
    public float menuZoom = 7.2f;

    private void LateUpdate()
    {
        if (!isFrozen && !isInMenu)
        {
            AddFloatyMovement();
        }
    }

    private void AddFloatyMovement()
    {
        if (!isFrozen)
        {
            float floatyOffsetX = Mathf.Sin(Time.time * floatySpeed) * floatyAmount;
            float floatyOffsetY = Mathf.Cos(Time.time * floatySpeed) * floatyAmount;
            Vector3 floatyTarget = new Vector3(
                transform.position.x + floatyOffsetX,
                transform.position.y + floatyOffsetY,
                transform.position.z
            );
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(floatyTarget.x, floatyTarget.y, -10),
                Time.deltaTime
            );
        }
    }

    public void Shake(float duration, float magnitude)
    {
        if (isFrozen)
        {
            return;
        }

        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SetTarget(Vector3 targetPosition, float targetZoom, bool isZoomIn)
    {
        // Transitioning to target
        targetPosition.z = -10;
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(
            SmoothTransition(targetPosition, targetZoom, isZoomIn)
        );
    }

    private IEnumerator SmoothTransition(Vector3 targetPosition, float targetZoom, bool isZoomIn)
    {
        float startZoom = GetComponent<Camera>().orthographicSize;
        Vector3 startPosition = transform.position;

        float elapsed = 0f;

        while (elapsed < zoomTransitionTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomTransitionTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // Interpolate the position smoothly
            currentTarget = Vector3.Lerp(startPosition, targetPosition, t);
            transform.position = currentTarget;

            // Interpolate the zoom smoothly
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

            yield return null;
        }

        // Set to exact target values
        transform.position = targetPosition;
        GetComponent<Camera>().orthographicSize = targetZoom;

        transitionCoroutine = null;

        if (isZoomIn)
            EventBus.Instance.TriggerCameraZoomIn();
        else
            EventBus.Instance.TriggerCameraZoomOut();
    }

    public void ZoomIn(Vector3 targetPosition)
    {
        SetTarget(targetPosition, zoomedInSize, true);
    }

    public void ZoomOut(Vector3 targetPosition)
    {
        SetTarget(targetPosition, zoomedOutSize, false);
    }

    public void SnapToPlayer(Vector3 playerPosition)
    {
        transform.position = new Vector3(playerPosition.x, playerPosition.y, -10);
    }
}
