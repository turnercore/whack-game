using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    Rigidbody2D rb;
    private WeaponSlot weaponSlot;
    private Weapon Weapon => weaponSlot.weapon;
    [SerializeField] private float MIN_FORCE_OF_IMPACT = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        // Get the bat's Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        weaponSlot = GetComponentInChildren<WeaponSlot>();
    }

    // On collision with an enemy, apply force to the enemy
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            // Debug.Log("Hit the enemy with force: " + other.relativeVelocity.magnitude);
            // See if the collision as t a minimum force
            if (other.relativeVelocity.magnitude < MIN_FORCE_OF_IMPACT)
            {
                // Debug.Log("Not enough force to hit the enemy");
                return;
            }
            // Get the normal
            Vector2 direction = other.contacts[0].normal;
            // Get the enemy's Rigidbody2D and enemy weight from its script
            Rigidbody2D enemyRb = other.collider.GetComponent<Rigidbody2D>();
            other.collider.GetComponent<Enemy>().Hit(direction, Weapon.damage, Weapon.addedForce);
        }
    }
}


