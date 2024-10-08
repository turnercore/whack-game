using System;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
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
    private PlayerAttack playerAttack;

    [SerializeField]
    private WeaponSlot weaponSlot;
    #endregion

    private void Start()
    {
        XP = 0;
        Level = 1;
        GameManager.Instance.RegisterPlayer(gameObject);
        health.OnDeath += OnDeath;
    }

    // Cleanup
    private void OnDestroy()
    {
        // Unsubscribe from events
        health.OnDeath -= OnDeath;
    }

    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }
        movement.MovePlayer();
    }

    public void AddXP(int xp)
    {
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
        Level++;
        XP = 0;
        EventBus.Instance.TriggerLevelUp();
    }

    // Detect collisions with Enemies
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy.IsDead)
            {
                return;
            }
            float damage = enemy.damage;
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
