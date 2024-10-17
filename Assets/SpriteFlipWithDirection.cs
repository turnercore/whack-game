using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipWithDirection : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        // Ensure the SpriteRenderer and rb is assigned (or find it automatically)
        if (spriteRenderer == null || rb == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Safety check for missing components
        if (rb == null || spriteRenderer == null)
        {
            Debug.LogError("Missing required components on " + gameObject.name);
        }
    }

    private void Update()
    {
        FlipSprite();
    }

    private void FlipSprite()
    {
        // Check the velocity to determine direction
        if (rb.velocity.x > 0)
        {
            // Moving to the right, ensure sprite is facing right
            spriteRenderer.flipX = true;
        }
        else if (rb.velocity.x < 0)
        {
            // Moving to the left, flip sprite
            spriteRenderer.flipX = false;
        }
    }
}
