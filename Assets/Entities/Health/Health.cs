using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 5;
    public float MaxHealth
    {
        get => (float)_maxHealth;
        set
        {
            // If new max health is higher than previous max health, get the difference
            float difference = value - _maxHealth;
            // Convert the float value to an integer rounded down (but above 0)
            int newValue = Mathf.Max(1, Mathf.FloorToInt(value));
            _maxHealth = newValue;
            // If the difference is positive, add the difference to current health
            if (difference > 0)
            {
                CurrentHealth += difference;
            }
            // Clamp current health to new value
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, (float)_maxHealth);
        }
    }
    public float CurrentHealth { get; private set; }
    public bool IsDead = false;
    public delegate void DeathDelegate();
    public event DeathDelegate OnDeath;
    public delegate void OnTakeDamageDelegate(float damage);
    public event OnTakeDamageDelegate OnTakeDamage;

    [SerializeField]
    private Animator EyeAnimator;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Call this function to take damage
    public void TakeDamage(float damage)
    {
        if (damage <= 0 || IsDead)
        {
            return;
        }
        CurrentHealth -= damage;
        // Clamp the value between 0 and MaxHealth
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // Run the OnHit event for anything that is subscribed to it
        OnTakeDamage?.Invoke(damage);
        // If the entity has an EyeAnimator Animator component, set the isShocked Trigger
        if (EyeAnimator != null)
        {
            EyeAnimator.SetTrigger("isShocked");
        }
        // If the enemy's health reaches 0, do something (e.g., destroy enemy)
        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        // Trigger the death event for anything that is subscribed
        // to the OnDeath event
        OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }
        CurrentHealth += amount;
        // Clamp the value between 0 and MaxHealth
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        // Run the OnHeal event for anything that is subscribed to it
        OnHeal?.Invoke(amount);
    }

    // OnHeal event taking the heal amount float
    public delegate void OnHealDelegate(float amount);
    public event OnHealDelegate OnHeal;

    public void Revive()
    {
        IsDead = false;
        CurrentHealth = MaxHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void SetHealth(float newHealth)
    {
        CurrentHealth = newHealth;
        // Clamp the value between 0 and MaxHealth
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
    }
}
