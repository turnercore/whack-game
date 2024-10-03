using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 Target => GameManager.Instance.GetPlayerObject().transform.position;

    private void LateUpdate()
    {
        transform.position = new Vector3(Target.x, Target.y, -10);
    }
}
