using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    public void MovePlayer()
    {
        // Get movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create movement vector
        Vector2 movement = new Vector2(horizontal, vertical).normalized * speed;

        // Move the player using Rigidbody2D MovePosition
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
}
