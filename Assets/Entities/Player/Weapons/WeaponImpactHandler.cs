using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponImpactHandler : MonoBehaviour
{
    private Rigidbody2D rb;
    private WeaponSlot weaponSlot;
    private Weapon weapon;
    private float previousRotationZ;
    private float angularVeloctiy =>
        Mathf.Abs(transform.rotation.z - previousRotationZ) / Time.fixedDeltaTime;

    // Start is called before the first frame update
    void Awake()
    {
        // Get the bat's Rigidbody2D component
        rb = GetComponentInParent<Rigidbody2D>();
        weaponSlot = GetComponentInParent<WeaponSlot>();
        weapon = GetComponent<Weapon>();
    }

    // On collision with an enemy, apply force to the enemy
    // yo
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            HitEnemy(other);
        }
    }

    void FixedUpdate()
    {
        previousRotationZ = transform.rotation.z;
    }

    void HitEnemy(Collision2D enemy)
    {
        // If the angular velocity is below a threshold, don't apply any force
        if (angularVeloctiy < 0.50f)
        {
            return;
        }
        weapon.PlayHitEffects();
        // Get the normal of the collision to determine the direction of the hit
        Vector2 direction = enemy.contacts[0].normal;
        // Get the added force as a function of the angular velocity
        // Should be a LERP with addedForce being weapon.addedForce at 5.0f angular velocity and 0 at 0.0f angular velocity
        float addedForce = Mathf.Lerp(0, weapon.addedForce, angularVeloctiy / 15.0f);

        enemy
            .collider.GetComponent<Enemy>()
            .Hit(
                direction,
                weapon.damage,
                weapon.multiplierMode,
                weapon.multiplierIncrease,
                weapon.startComboMultiplier,
                addedForce,
                weapon.addedWackedTime
            );
    }
}
