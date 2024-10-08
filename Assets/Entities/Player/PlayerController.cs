using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float rotateSpeed = 4.0f;
    public int Level { get; private set; }
    public int XP { get; private set; }
    public bool IsDead => health.IsDead;
    public int MaxXP => 10;

    [Category("Linked Components")]
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Health health;

    [SerializeField]
    private PlayerMovement movement;

    [SerializeField]
    private Rigidbody2D weaponRigidBody;

    [SerializeField]
    private Transform weaponPivot;

    [SerializeField]
    private WeaponSlot weaponSlot;
    #endregion

    private void Start()
    {
        XP = 0;
        Level = 1;
        GameManager.Instance.RegisterPlayer(gameObject);
        // Subscribe to Health events
        health.OnDeath += OnDeath;
    }

    // Cleanup
    private void OnDestroy()
    {
        // Unsubscribe from Health events
        health.OnDeath -= OnDeath;
    }

    private void FixedUpdate()
    {
        // Check if the player is dead
        if (IsDead)
        {
            return;
        }
        movement.MovePlayer();
        PivotWeapon();
    }

    private void PivotWeapon()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we stay in 2D
        // Get direction from player to mouse
        Vector2 direction = (mousePos - weaponPivot.position).normalized;
        // Calculate the angle between the player and the mouse position
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Smoothly rotate the weapon using Rigidbody2D
        float currentAngle = weaponRigidBody.rotation;
        float newAngle = Mathf.LerpAngle(
            currentAngle,
            targetAngle,
            rotateSpeed * Time.fixedDeltaTime
        );
        // Debug.Log(Mathf.Abs(newAngle - currentAngle) < 0.1f);
        // Apply the new rotation using Rigidbody2D.MoveRotation
        weaponRigidBody.MoveRotation(newAngle);
    }

    public void AddXP(int xp)
    {
        // Add the xp to the player's current xp
        XP += xp;

        // Trigger the XPChanged event
        EventBus.Instance.TriggerXPChanged();

        // Check if the player has enough xp to level up
        if (XP >= MaxXP)
        {
            // If the player has enough xp, level up the player
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increase the player's level by 1
        Level++;
        // Reset the player's xp to 0
        XP = 0;
        EventBus.Instance.TriggerLevelUp();
    }

    // Detect collisions with Enemies
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy component
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            // Check if enemy is dead
            if (enemy.IsDead)
            {
                return;
            }
            // Get the enemy's damage value
            float damage = enemy.damage;
            // Take damage from the enemy
            health.TakeDamage(damage);
        }
    }

    private void OnDeath()
    {
        // Unsubscribe from Health events
        health.OnDeath -= OnDeath;
        // Trigger the player died event
        EventBus.Instance.TriggerPlayerDied();
    }

    public void Heal(float amount)
    {
        health.Heal(amount);
    }

    public void AddCoins(int coins)
    {
        EventBus.Instance.TriggerCoinCollected(coins);
    }

    public void AddWeapon(GameObject weapon)
    {
        weaponSlot.SetWeapon(weapon);
    }

    public void ResetPlayer()
    {
        XP = 0;
        Level = 1;
        health.ResetHealth();
    }
}
