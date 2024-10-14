using UnityEngine;

public class WeaponImpactHandler : MonoBehaviour
{
    Rigidbody2D rb;
    private WeaponSlot weaponSlot;
    private Weapon Weapon => weaponSlot.weapon;

    [SerializeField]
    private float MIN_FORCE_OF_IMPACT = 5.0f;

    // Start is called before the first frame update
    void Awake()
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
            HitEnemy(other);
        }
    }

    void HitEnemy(Collision2D enemy)
    {
        Weapon.PlayHitEffects();
        // Check if the enemy's relative velocity is less than the minimum force of impact, if so, return
        if (enemy.relativeVelocity.magnitude < MIN_FORCE_OF_IMPACT)
        {
            return;
        }
        // Get the normal of the collision to determine the direction of the hit
        Vector2 direction = enemy.contacts[0].normal;
        // Hit the enemy
        enemy
            .collider.GetComponent<Enemy>()
            .Hit(
                direction,
                Weapon.damage,
                Weapon.multiplierMode,
                Weapon.multiplierIncrease,
                Weapon.startComboMultiplier,
                Weapon.addedForce,
                Weapon.addedWackedTime
            );
    }
}
