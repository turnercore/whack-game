using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    [SerializeField]
    private Health health;

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
}
