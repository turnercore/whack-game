using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public Vector2 Direction;
    private bool isMovementBlocked = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void MovePlayer()
    {
        if (isMovementBlocked)
            return;

        // Get movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Set direction to the way the placer is 'facing' or trying to move
        Vector2 movement = new Vector2(horizontal, vertical).normalized * speed;

        // Set the direction to the movement vector, if the player isn't moving the Direction is still set to the last direction
        if (movement.magnitude > 0)
            Direction = movement;

        // Move the player using Rigidbody2D MovePosition
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }

    public void BlockMovement()
    {
        isMovementBlocked = true;
    }

    public void UnblockMovement()
    {
        isMovementBlocked = false;
    }
}
