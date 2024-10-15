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
    public Rigidbody2D rb;

    [SerializeField]
    private Health health;

    [SerializeField]
    private PlayerMovement movement;

    [SerializeField]
    private PlayerAttack playerAttack;

    [SerializeField]
    private WeaponSlot weaponSlot;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private SpriteRenderer ghostSprite;

    private Weapon currentWeapon;
    #endregion

    private void Awake()
    {
        XP = 0;
        Level = 1;
        health.OnDeath += OnDeath;
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(gameObject);
        if (weaponSlot != null && weaponSlot.weapon != null)
        {
            AddWeapon(weaponSlot.weapon.gameObject);
        }
    }

    // Cleanup
    private void OnDestroy()
    {
        // Unsubscribe from events
        health.OnDeath -= OnDeath;
    }

    private void Update()
    {
        if (IsDead && !movement.IsMovementBlocked)
        {
            movement.BlockMovement();
        }
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

    private void OnDeath()
    {
        // Change the sprite to the ghost sprite
        sprite.gameObject.SetActive(false);
        ghostSprite.gameObject.SetActive(true);
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
        currentWeapon = weaponSlot.SetWeapon(weapon);
        currentWeapon.Initialize(
            playerAttack.damage,
            playerAttack.force,
            playerAttack.startComboMultiplier,
            playerAttack.comboIncrease,
            playerAttack.comboMultiplierMode,
            playerAttack.addedWackedTime
        );
        playerAttack.SetWeapon(currentWeapon);
        movement.SetWeapon(currentWeapon);
    }

    public void ResetPlayer()
    {
        XP = 0;
        Level = 1;
        health.ResetHealth();
    }

    public void BlockMovement()
    {
        movement.BlockMovement();
    }

    public void UnblockMovement()
    {
        movement.UnblockMovement();
    }

    public void TeleportTo(Vector3 position)
    {
        gameObject.transform.position = position;
        rb.position = position;
    }

    public Vector3 GetPosition()
    {
        return rb.position;
    }
}
