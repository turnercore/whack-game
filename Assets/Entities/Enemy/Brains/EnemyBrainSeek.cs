using UnityEngine;

public class EnemyBrainSeek : EnemyBrain
{
    protected override void Act()
    {
        // Get the direction towards the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Move towards the player
        MoveTowards(direction);
    }
}
