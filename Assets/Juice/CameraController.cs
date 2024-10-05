using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float deadzone = 0.5f;
    [SerializeField] private float floatyAmount = 0.02f;
    [SerializeField] private float floatySpeed = 0.2f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float idleThreshold = 1f;
    private Vector3 targetPosition;
    private Transform playerTransform;
    private float playerIdleTime = 0f;
    private Vector3 lastPlayerPosition;
    private bool isIdle = false;

    private void Start()
    {
        playerTransform = GameManager.Instance.GetPlayerObject().transform;
        offset = transform.position - playerTransform.position;
        lastPlayerPosition = playerTransform.position;
    }

    private void LateUpdate()
    {
        FollowPlayerWithDeadzone();
        CheckPlayerIdle();
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
            targetPosition = Vector3.Lerp(camPosition, playerPosition, followSpeed * Time.deltaTime);
            transform.position = new Vector3(targetPosition.x + offset.x, targetPosition.y + offset.y, -10);
            playerIdleTime = 0f; // Reset idle time since player is moving
            isIdle = false;
        }
    }
    private void CheckPlayerIdle()
    {
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
        if (isIdle)
        {
            float floatyOffsetX = Mathf.Sin(Time.time * floatySpeed) * floatyAmount;
            float floatyOffsetY = Mathf.Cos(Time.time * floatySpeed) * floatyAmount;
            Vector3 floatyTarget = new Vector3(transform.position.x + floatyOffsetX, transform.position.y + floatyOffsetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, floatyTarget, Time.deltaTime);
        }
    }

    public void Shake(float duration, float magnitude)
    {
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
}