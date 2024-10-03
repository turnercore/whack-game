using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        // Follow the player's position
        if (player != null)
        {
            target = player.transform;
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
