using System;
using System.Collections;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    public Rigidbody2D rb;
    public float rotationSpeed = 200f; // Speed of rotation
    private Enemy enemy;
    private bool IsDead => enemy.IsDead;

    public void Initialize(Rigidbody2D rb, Enemy enemy)
    {
        this.rb = rb;
        this.enemy = enemy;
    }

    public void Disable()
    {
        // If the enemy is dead it no longer needs to rotate upright
        if (IsDead)
        {
            return;
        }

        // Freeze rotation on Rigidbody
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Start coroutine to smoothly rotate upright
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(RotateUpright());
        }
    }

    public void Enable()
    {
        // Allow enemy to move around
        rb.constraints = RigidbodyConstraints2D.None; // Remove constraints to allow movement and rotation

        // Stop any ongoing rotation when enabling
        StopAllCoroutines();
    }

    public IEnumerator RotateUpright()
    {
        // Target upright rotation
        Quaternion targetRotation = Quaternion.identity;

        // Smoothly rotate until almost upright
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            // Rotate towards target rotation gradually
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Ensure final rotation is exactly upright
        transform.rotation = targetRotation;
    }

    internal void Initalize(Rigidbody2D rb)
    {
        throw new NotImplementedException();
    }
}
