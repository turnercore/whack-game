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
            HitEnemy(other);
        }
    }

    void HitEnemy(Collision2D enemy)
    {
        // Debug.Log("Hit the enemy with force: " + other.relativeVelocity.magnitude);
            // See if the collision as t a minimum force
            if (enemy.relativeVelocity.magnitude < MIN_FORCE_OF_IMPACT)
            {
                // Debug.Log("Not enough force to hit the enemy");
                return;
            }
            // Get the normal
            Vector2 direction = enemy.contacts[0].normal;
            // Get the enemy's Rigidbody2D and enemy weight from its script
            Rigidbody2D enemyRb = enemy.collider.GetComponent<Rigidbody2D>();
            enemy.collider.GetComponent<Enemy>().Hit(direction, Weapon.damage, Weapon.addedForce);

            // Play the hit sound
            Weapon.PlayHitEffects();
    }
}


