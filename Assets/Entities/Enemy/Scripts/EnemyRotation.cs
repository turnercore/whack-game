using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rotationSpeed = 200f; // Speed of rotation
    private Enemy enemy;
    private bool IsDead => enemy.IsDead;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Awake()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        // If the enemy is dead it no longer needs to rotate upright
        if (IsDead)
        {
            return;
        }

        // Freeze rotation on Rigidbody
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Start coroutine to smoothly rotate upright
        if (gameObject.activeInHierarchy) {
            StartCoroutine(RotateUpright());
        }
    }



    private void OnEnable()
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure final rotation is exactly upright
        transform.rotation = targetRotation;
    }
}
