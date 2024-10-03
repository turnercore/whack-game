using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyBrain))]
[RequireComponent(typeof(EnemyRotation))]
[RequireComponent(typeof(OffScreenChecker))]
public class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField] private float weight = 1.0f;
    public float speed = 1.0f;
    [SerializeField] private float wackedTime = 0.5f;
    public bool IsWacked = false;
    private bool hasFirstDeathStop = false;
    public float DamageHit { get; private set; }
    public Vector2 DirectionHit { get; private set; }
    public float AddedForceHit { get; private set; }
    public bool IsDead => health.IsDead;
    [Header("Linked Components")]
    public Rigidbody2D rb;
    [SerializeField] private Health health;
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyRotation enemyRotation;
    [SerializeField] private EnemyDrops enemyDrops;
    [SerializeField] private GameObject triggerCollider;
    [SerializeField] private OffScreenChecker offScreenChecker;



    #endregion

    void Start()
    {
        // Get references to components if they aren't set
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (health == null)
        {
            health = GetComponent<Health>();
        }
        if (brain == null)
        {
            brain = GetComponent<EnemyBrain>();
        }
        if (enemyRotation == null)
        {
            enemyRotation = GetComponent<EnemyRotation>();
        }
        if (enemyDrops == null)
        {
            enemyDrops = GetComponent<EnemyDrops>();
        }
        if (offScreenChecker == null)
        {
            offScreenChecker = GetComponent<OffScreenChecker>();
        }
        // Subscribe to off-screen status changes
        offScreenChecker.OnScreenStatusChanged += OnScreenStatusChanged;
        // Subscribe to health ondeath
        health.OnDeath += Die;
        // Disable rotation
        enemyRotation.enabled = false;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        health.OnDeath -= Die;
        offScreenChecker.OnScreenStatusChanged -= OnScreenStatusChanged;
    }

    private void OnScreenStatusChanged(bool isOffScreen)
    {
        // If the enemy is off-screen, re-enable brain
        if (isOffScreen)
        {
            EnableBrain();
        }

        // If dead and off-screen, destroy the enemy
        if (IsDead && isOffScreen)
        {
            Unload();
        }
    }

    void Die()
    {
        // Drop loot
        enemyDrops.DropLoot();
        // Disable the brain
        brain.enabled = false;
        // Disable the rotation script
        enemyRotation.enabled = true;
        // Fire enemy is dead event
        EventBus.Instance.EnemyDied(this);
    }

    public void Hit(Vector2 direction, float damage, float addedForce = 1.0f, float addedWackedTime = 0.0f)
    {
        // If the enemy is already being hit, ignore the new hit, if it's dead reset IsWacked
        if (IsWacked)
        {
            if (IsDead) StartCoroutine(ResetIsWacked(0));
            return;
        }

        // Otherwise, handle the hit, update IsWacked, and start the coroutine
        Wacked(addedWackedTime);
        DamageHit = damage;
        DirectionHit = direction;
        AddedForceHit = addedForce;
        HandleHit(direction, damage, addedForce);
    }
    private void HandleHit(Vector2 direction, float damage, float force = 1.0f)
    {
        // Take damage
        health.TakeDamage(damage);
        // Add force to the enemy in the direction of the hit
        rb.AddForce(force * direction, ForceMode2D.Impulse);
    }
    private void Wacked(float addedWackedTime = 0.0f)
    {
        IsWacked = true;
        // Turn off brain when wacked until either off-screen or a short time has passed, unless dead then it will remain disabled
        if (brain != null && brain.enabled && !IsDead)
        {
            StartCoroutine(DisableBrainTemporarily());
        }
        // Unfreeze enemy rotation
        enemyRotation.enabled = true;
        // Enable other enemy detection 
        triggerCollider.SetActive(true);
        StartCoroutine(ResetIsWacked(wackedTime + addedWackedTime));
    }

    private IEnumerator ResetIsWacked(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        IsWacked = false;
        // Freeze enemy rotation
        enemyRotation.enabled = false;
        // Disable other enemy detection
        triggerCollider.SetActive(false);
        // If the enemy is dead, slowly lower the velocity to 0
        if (IsDead && !hasFirstDeathStop)
        {
            hasFirstDeathStop = true;
            while (rb.velocity.magnitude > 0.1f)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.1f);
                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.velocity = Vector2.zero;
                }
                yield return null;
            }
        }
    }

    private IEnumerator DisableBrainTemporarily()
    {
        brain.enabled = false;
        if (IsDead)
        {
            yield break;
        }

        while (IsWacked)
        {
            // Check if the enemy is off-screen, and re-enable the brain immediately
            if (!offScreenChecker.IsOnScreen)
            {
                EnableBrain();
                yield break;
            }
            if (IsDead)
            {
                break;
            }
            if (!IsWacked)
            {
                break;
            }

            yield return null;
        }

        // Re-enable the brain if the off-screen condition was not met
        EnableBrain();
    }
    // Enable the brain
    private void EnableBrain()
    {
        if (brain != null)
        {
            brain.enabled = true;
        }
    }
    // Script to get rid of the enemy, maybe uses pooling, etc.
    private void Unload()
    {
        Destroy(gameObject);
    }
}
