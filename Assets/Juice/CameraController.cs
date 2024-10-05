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
    private float followSpeed = 5f;

    [SerializeField]
    private float deadzone = 0.5f;

    [SerializeField]
    private float floatyAmount = 0.02f;

    [SerializeField]
    private float floatySpeed = 0.2f;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float zoomTransitionTime = 5f;

    [SerializeField]
    private float zoomedInSize = 3f;

    [SerializeField]
    private float zoomedOutSize = 7.2f;

    [SerializeField]
    private float idleThreshold = 1f;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private float playerIdleTime = 0f;
    private Vector3 lastPlayerPosition;
    private bool isIdle = false;
    public bool isInMenu = false;

    private Coroutine zoomCoroutine;
    private float zoomElapsedTime = 0f;

    [SerializeField]
    private bool _zoomIn = false;
    public bool zoomIn
    {
        get { return _zoomIn; }
        set
        {
            _zoomIn = value;
            _zoomOut = !value;

            // Reset and start coroutine if necessary
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ZoomToSize(zoomedInSize));
        }
    }

    [SerializeField]
    private bool _zoomOut = false;
    public bool zoomOut
    {
        get { return _zoomOut; }
        set
        {
            _zoomOut = value;
            _zoomIn = !value;

            // Reset and start coroutine if necessary
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            float targetSize = isInMenu ? menuZoom : zoomedOutSize;
            zoomCoroutine = StartCoroutine(ZoomToSize(targetSize));
        }
    }

    private Vector3 menuPosition;
    private float menuZoom;
    private Quaternion menuRotation;

    private void Start()
    {
        playerTransform = GameManager.Instance.GetPlayerObject().transform;
        lastPlayerPosition = playerTransform.position;
        // Record the starting position, zoom, and rotation of the camera and save it for menus
        menuPosition = transform.position;
        menuZoom = GetComponent<Camera>().orthographicSize;
        menuRotation = transform.rotation;
        //Snap zoomed in on player to start
        SnapZoomInOnPlayer();
    }

    private void LateUpdate()
    {
        if (!isFrozen && !isInMenu && playerTransform != null)
        {
            FollowPlayerWithDeadzone();
            CheckPlayerIdle();
        }
    }

    // Set to immedetly zoomed in on player function
    public void SnapZoomInOnPlayer()
    {
        _zoomIn = true;
        _zoomOut = false;
        transform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            -10
        );
        GetComponent<Camera>().orthographicSize = 3f;
    }

    // Set to immedetly zoomed out on player function
    public void SnapZoomOutOnPlayer()
    {
        _zoomOut = true;
        _zoomIn = false;
        transform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            -10
        );
        GetComponent<Camera>().orthographicSize = 7.2f;
    }

    private void FollowPlayerWithDeadzone()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 camPosition = transform.position - offset;

        // Calculate the distance between the camera position and the player position
        float distanceX = Mathf.Abs(playerPosition.x - camPosition.x);
        float distanceY = Mathf.Abs(playerPosition.y - camPosition.y);

        // If the player is outside the deadzone, move the camera towards the player
        if (distanceX > deadzone || distanceY > deadzone)
        {
            targetPosition = Vector3.Lerp(
                camPosition,
                playerPosition,
                followSpeed * Time.deltaTime
            );
            transform.position = new Vector3(
                targetPosition.x + offset.x,
                targetPosition.y + offset.y,
                -10
            );
            playerIdleTime = 0f; // Reset idle time since player is moving
            isIdle = false;
        }
    }

    private void CheckPlayerIdle()
    {
        if (isFrozen)
        {
            return;
        }

        if (playerTransform.position == lastPlayerPosition)
        {
            playerIdleTime += Time.deltaTime;
            if (playerIdleTime >= idleThreshold)
            {
                isIdle = true;
                AddFloatyMovement();
            }
        }
        else
        {
            playerIdleTime = 0f;
            isIdle = false;
        }

        lastPlayerPosition = playerTransform.position;
    }

    private void AddFloatyMovement()
    {
        if (isIdle && !isFrozen)
        {
            float floatyOffsetX = Mathf.Sin(Time.time * floatySpeed) * floatyAmount;
            float floatyOffsetY = Mathf.Cos(Time.time * floatySpeed) * floatyAmount;
            Vector3 floatyTarget = new Vector3(
                transform.position.x + floatyOffsetX,
                transform.position.y + floatyOffsetY,
                transform.position.z
            );
            transform.position = Vector3.Lerp(transform.position, floatyTarget, Time.deltaTime);
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

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Freeze camera movement
    public void Freeze()
    {
        isFrozen = true;
    }

    // Unfreeze camera movement
    public void Unfreeze()
    {
        isFrozen = false;
    }

    private IEnumerator ZoomToSize(float targetZoom)
    {
        float startZoom = GetComponent<Camera>().orthographicSize;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        // Target values for position and rotation depending on menu state
        Vector3 targetPosition = isInMenu
            ? menuPosition
            : new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        Quaternion targetRotation = isInMenu ? menuRotation : Quaternion.identity;

        zoomElapsedTime = 0f;

        while (zoomElapsedTime < zoomTransitionTime)
        {
            zoomElapsedTime += Time.deltaTime;
            float t = zoomElapsedTime / zoomTransitionTime;

            // Smoothly interpolate the zoom based on time
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(startZoom, targetZoom, t);

            // Smoothly interpolate the position
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Smoothly interpolate the rotation
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Ensure we hit the target values exactly at the end
        GetComponent<Camera>().orthographicSize = targetZoom;
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // Reset the coroutine reference to indicate it's completed
        zoomCoroutine = null;
    }
}
