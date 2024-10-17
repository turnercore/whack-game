using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField]
    private float swayAmount = 0.05f;

    [SerializeField]
    private float swaySpeed = 1.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        float swayOffsetX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        float swayOffsetY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;

        transform.position = initialPosition + new Vector3(swayOffsetX, swayOffsetY, 0);
    }
}
